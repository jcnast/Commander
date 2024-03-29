﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BaseUnit : MonoBehaviour {

	// states the unit can be in
	public enum UnitState{
		Waiting,
		RecievingOrders,
		DoingOrderOne,
		ChoosingOrderTwo,
		DoingOrderTwo,
	}

	// different types of possible orders
	public enum OrderType{
		Null,
		Hold,
		Move,
		Attack,
	}

	// the type of attacks possible
	public enum AttackType{
		Circle,
		Line,
		Arc
	}

	// all UI components
	public GameObject orderUI;
	public Button orderOneButton;
	public Button orderTwoAButton;
	public Button orderTwoBButton;
	public Button holdCommand;
	public Button moveCommand;
	public Button attackCommand;

	// gameobject indicators
	public GameObject orderOneIndicator;
	public GameObject orderTwoAIndicator;
	public GameObject orderTwoBIndicator;

	// sprites
	public Sprite nullSprite;
	public Sprite holdSprite;
	public Sprite moveSprite;
	public Sprite attackSprite;

	// singleton values that get references regularily
	protected InputManager inputManager;
	protected PathManager pathManager;

	// info about position on board (and in world)
	protected BaseTile curTile;
	protected float spriteExtents;

	// general unit information
	public float maxHealth;
	public float maxDamage;
	public int maxMove;
	public int maxInitiative; // higher initiatives go first

	// unity attack information
	public AttackType attackType;
	public int attackRange = 0;
	public int attackNum = 0;

	// current state of the unit
	protected UnitState curState;

	// future position (if first command is move)
	protected BaseTile nextTile;

	// orders and their target positions
	protected OrderType orderOne = OrderType.Null; // 1
	protected List<Vector3> orderOnePosition = new List<Vector3>();
	protected OrderType orderTwoA = OrderType.Null; // 2
	protected List<Vector3> orderTwoAPosition = new List<Vector3>();
	protected OrderType orderTwoB = OrderType.Null; // 3
	protected List<Vector3> orderTwoBPosition = new List<Vector3>();

	// active order information
	protected int activeOrder = 0;
	protected OrderType activeCommand = OrderType.Null;
	protected List<PathManager.PathTile> activePathTiles = new List<PathManager.PathTile>();

	/* 
	*******************************************
			Standard Unity Functions
	*******************************************
	*/

	void OnDestroy(){
		// remove event listeners
		Events.instance.RemoveListener<IssueOrdersEvent> (IssueOrders);
		Events.instance.RemoveListener<TileClickedEvent> (TileClicked);
		Events.instance.RemoveListener<UnitClickedEvent> (UnitClicked);
	}

	void Awake(){
		// add event listeners
		Events.instance.AddListener<IssueOrdersEvent> (IssueOrders);
		Events.instance.AddListener<TileClickedEvent> (TileClicked);
		Events.instance.AddListener<UnitClickedEvent> (UnitClicked);

		// get input manager and set UI to false
		inputManager = InputManager.Instance;
		pathManager = PathManager.Instance;
		orderUI.SetActive(false);
		CommandsInteractable(false);
		OrdersInteractable(true);

		// UI button delegate assignment
		orderOneButton.onClick.AddListener(() => {ChangeActiveOrder(1);});
		orderTwoAButton.onClick.AddListener(() => {ChangeActiveOrder(2);});
		orderTwoBButton.onClick.AddListener(() => {ChangeActiveOrder(3);});
		holdCommand.onClick.AddListener(() => {SetOrderCommand(OrderType.Hold);});
		moveCommand.onClick.AddListener(() => {SetOrderCommand(OrderType.Move);});
		attackCommand.onClick.AddListener(() => {SetOrderCommand(OrderType.Attack);});

		// get the size of the sprite
		spriteExtents = transform.GetComponent<SpriteRenderer>().bounds.extents.x;

		// set current unit state
		curState = UnitState.Waiting;
	}

	void Update(){
		// check if UI was clicked on
		if(!inputManager.UISelected){
			// check if mouse is within unit bounds
			if(inputManager.WorldCursorPosition.x >= transform.position.x - spriteExtents &&
				inputManager.WorldCursorPosition.x <= transform.position.x + spriteExtents &&
				inputManager.WorldCursorPosition.y >= transform.position.y - spriteExtents &&
				inputManager.WorldCursorPosition.y <= transform.position.y + spriteExtents){
				// check if click was released
				if(inputManager.ClickUp){
					Events.instance.Raise( new UnitClickedEvent (transform));
				}
			}
		}
	}

	/* 
	*******************************************
			Click Event Listeners
	*******************************************
	*/

	// Issue orders event recieves
	void IssueOrders(IssueOrdersEvent e){
		// set state to recieving orders
		curState = UnitState.RecievingOrders;
	}

	void TileClicked(TileClickedEvent e){
		if(curState == UnitState.RecievingOrders){
			// if activePathTiles exist
			if(activePathTiles.Count > 0){
				// determine if clicked tile is active
				int index = activePathTiles.FindIndex((x) => (x.CurrentTile == e.baseTile));
				if(index >= 0){
					// if tile is active, set position of order
					List<Vector3> orderPath = new List<Vector3>();

					// follow Path Tiles back to null to create list of path tiles
					while(activePathTiles[index].CurrentTile != null){
						// add tile to path
						orderPath.Add(activePathTiles[index].CurrentTile.transform.position);
						// find next tile
						index = activePathTiles.FindIndex(x => x.CurrentTile == activePathTiles[index].PreviousTile);
						// if no more in path, stop
						if(index < 0){
							break;
						}
					}
					SetOrderValues(orderPath, e.baseTile);

					// and clear active tiles (as one was chosen)
					ClearActiveTiles();
				}else{ // Tile is not a member of activePathTiles
					// do nothing
				}
			}else{
				if(orderUI.activeSelf){// if UI is open, close UI
					orderUI.SetActive(false);
				}else if(activePathTiles.Count != 0){// if tiles are active
					// do nothing
				}
			}
		}
	}

	void UnitClicked(UnitClickedEvent e){
		// always clear clickable tiles 
		ClearActiveTiles();
		if(e.Unit == transform){
			if(curState == UnitState.Waiting){
				// nothing?
			}else if(curState == UnitState.RecievingOrders){
				// display order UI
				orderUI.SetActive(true);

				// set what can be clicked on
				CommandsInteractable(false);
				OrdersInteractable(true);

				// show the order indicators
				ShowIndicators(true);
			}else if(curState == UnitState.DoingOrderOne){
				// nothing?
			}else if(curState == UnitState.ChoosingOrderTwo){
				// show orders/start process
			}else if(curState == UnitState.DoingOrderTwo){
				// nothing?
			}
		}else{
			// different unit was selected, remove UI
			orderUI.SetActive(false);

			// hide the indicators
			ShowIndicators(false);
		}
	}

	/* 
	*******************************************
				Button Functions
	*******************************************
	*/

	// Change the active order according to what button was pressed
	void ChangeActiveOrder(int currentOrder){
		// raise event that the unit has been ordered in
		Events.instance.Raise( new UnitOrderedInEvent (transform));

		// set the current active order
		activeOrder = currentOrder;

		// set buttons to interactable
		OrdersInteractable(false);
		CommandsInteractable(true);
	}

	// Change the chosen command according to what button was pressed
	void SetOrderCommand(OrderType command){
		// set the current active command
		activeCommand = command;

		// set buttons to non-interactable
		CommandsInteractable(false);
		OrdersInteractable(true);

		// hide UI
		orderUI.SetActive(false);

		if(command == OrderType.Hold){
			DeterminePosition(0);
		}else if(command == OrderType.Move){
			DeterminePosition(maxMove);
		}else if(command == OrderType.Attack){
			DetermineAttackPosition(attackRange);
		}
	}

	/* 
	*******************************************
				Command Functions
	*******************************************
	*/

	// highlight all tiles within range
	void DeterminePosition(int numSquares){
		// if this is the second order, use nextTile where appropriate
		BaseTile tempTile;
		if(curTile == nextTile){
			tempTile = curTile;
		}else{
			tempTile = nextTile;
		}

		// if numSquares is 0, return current tile
		if(numSquares == 0){
			List<Vector3> selfPositionList = new List<Vector3>();
			selfPositionList.Add(tempTile.transform.position);
			SetOrderValues(selfPositionList, tempTile);
		}else{
			// find active path tiles
			activePathTiles = pathManager.FindMovementTiles(tempTile, maxMove);

			// display the tiles that can be clicked on
			ShowActiveTiles(true);
		}
	}

	protected virtual void DetermineAttackPosition(int Range){
		// each unit should have it's own function
	}

	// set each order's command and go-to position
	void SetOrderValues(List<Vector3> position, BaseTile tile){
		// determine what sprite to use for representation of command
		Sprite curSprite = nullSprite;
		if(activeCommand == OrderType.Hold){
			curSprite = holdSprite;
		}else if(activeCommand == OrderType.Move){
			curSprite = moveSprite;
		}else if(activeCommand == OrderType.Attack){
			curSprite = attackSprite;
		}

		if(activeOrder == 1){
			if(activeCommand == OrderType.Move){
				if(nextTile == tile){
					// do nothing
				}else{
					// set new nextTile
					nextTile = tile;
					// and clear other orders

					orderTwoA = OrderType.Null;
					orderTwoAPosition = null;
					orderTwoAButton.GetComponent<Image>().sprite = nullSprite;

					orderTwoB = OrderType.Null;
					orderTwoBPosition = null;
					orderTwoBButton.GetComponent<Image>().sprite = nullSprite;
				}
			}

			// set values of orderOne
			orderOne = activeCommand;
			orderOnePosition = position;

			// change sprite to indicate order's command
			orderOneButton.GetComponent<Image>().sprite = curSprite;

			// move the indicator to the specific tile with appropriate image
			orderOneIndicator.GetComponent<SpriteRenderer>().sprite = curSprite;
			orderOneIndicator.transform.position = tile.transform.position;
		}else if(activeOrder == 2){
			// set values of orderTwoA
			orderTwoA = activeCommand;
			orderTwoAPosition = position;

			// change sprite to indicate order's command
			orderTwoAButton.GetComponent<Image>().sprite = curSprite;

			// move the indicator to the specific tile with appropriate image
			orderTwoAIndicator.GetComponent<SpriteRenderer>().sprite = curSprite;
			orderTwoAIndicator.transform.position = tile.transform.position;
		}else if(activeOrder == 3){
			// set values of orderTwoB
			orderTwoB = activeCommand;
			orderTwoBPosition = position;

			// change sprite to indicate order's command
			orderTwoBButton.GetComponent<Image>().sprite = curSprite;

			// move the indicator to the specific tile with appropriate image
			orderTwoBIndicator.GetComponent<SpriteRenderer>().sprite = curSprite;
			orderTwoBIndicator.transform.position = tile.transform.position;
		}
		// display the indicators as they can be shown now
		ShowIndicators(true);

		// if the unit is ordered out, raise event
		if(orderOne != OrderType.Null && orderTwoA != OrderType.Null && orderTwoB != OrderType.Null){
			Events.instance.Raise( new UnitOrderedOutEvent (transform));
		}else{// otherwise, show the UI to choose the remaining orders
			// show the UI for the unit again
			orderUI.SetActive(true);

			// with the appropriate clickable things
			CommandsInteractable(false);
			OrdersInteractable(true);
		}
	}

	// set the order interactability
	void OrdersInteractable(bool interact){
		orderOneButton.interactable = interact;
		if(orderOne != OrderType.Null){
			orderTwoAButton.interactable = interact;
			orderTwoBButton.interactable = interact;
		}else{
			orderTwoAButton.interactable = false;
			orderTwoBButton.interactable = false;
		}
	}

	// set the command interactability
	void CommandsInteractable(bool interact){
		holdCommand.interactable = interact;
		moveCommand.interactable = interact;
		if(curTile == null || curTile.TypeOfTile == BaseTile.TileType.Water){
			attackCommand.interactable = false;
		}else{
			attackCommand.interactable = interact;
		}
	}

	// set the indicators visibility
	void ShowIndicators(bool visible){
		if(orderOne != OrderType.Null){
			orderOneIndicator.SetActive(visible);
		}else{
			orderOneIndicator.SetActive(false);
		}

		if(orderTwoA != OrderType.Null){
			orderTwoAIndicator.SetActive(visible);
		}else{
			orderTwoAIndicator.SetActive(false);
		}

		if(orderTwoB != OrderType.Null){
			orderTwoBIndicator.SetActive(visible);
		}else{
			orderTwoBIndicator.SetActive(false);
		}
	}

	// light/de-light active tiles
	protected void ShowActiveTiles(bool show){
		for(int i = 0; i < activePathTiles.Count; i++){
			activePathTiles[i].CurrentTile.LightUp(show);
		}
	}

	protected void ClearActiveTiles(){
		activeOrder = 0;
		activeCommand = OrderType.Null;
		ShowActiveTiles(false);
		activePathTiles.Clear();
	}

	/* 
	*******************************************
			Publicly Available Variables
	*******************************************
	*/

	// tile tjat unit is currently on
	public BaseTile CurTile{
		get {return curTile;}
		set {curTile = value;
			 nextTile = value;}
	}

	public UnitState CurState{
		get {return curState;}
		set {curState = value;}
	}

	public OrderType OrderOne{
		get {return orderOne;}
	}

	public List<Vector3> OrderOnePosition{
		get {return orderOnePosition;}
	}

	public OrderType OrderTwoA{
		get {return orderTwoA;}
	}

	public List<Vector3> OrderTwoAPosition{
		get {return orderTwoAPosition;}
	}

	public OrderType OrderTwoB{
		get {return orderTwoB;}
	}

	public List<Vector3> OrderTwoBPosition{
		get {return orderTwoBPosition;}
	}
}
