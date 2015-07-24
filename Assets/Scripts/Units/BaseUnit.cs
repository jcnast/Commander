using UnityEngine;
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
	public Sprite nullSprite;
	public Button holdCommand;
	public Sprite holdSprite;
	public Button moveCommand;
	public Sprite moveSprite;
	public Button attackCommand;
	public Sprite attackSprite;

	// singleton values that get references regularily
	private InputManager inputManager;
	private int numRows;
	private int numCols;
	private GameObject[,] mapTiles;

	// info about position on board (and in world)
	private BaseTile curTile;
	private float spriteExtents;

	// general unit information
	public float maxHealth;
	public float maxDamage;
	public int maxMove;
	public int maxInitiate; // higher initiatives go first

	// unity attack information
	public AttackType attackType;
	public int attackRange = 0;
	public int attackNum = 0;

	// current state of the unit
	private UnitState curState;

	// orders and their target positions
	private OrderType orderOne; // 1
	private Vector3 orderOnePosition = Vector3.zero;
	private OrderType orderTwoA; // 2
	private Vector3 orderTwoAPosition = Vector3.zero;
	private OrderType orderTwoB; // 3
	private Vector3 orderTwoBPosition = Vector3.zero;

	// active order information
	private int activeOrder = 0;
	private OrderType activeCommand = OrderType.Null;
	private List<GameObject> activeTiles = new List<GameObject>();

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

		// get Map Tiles
		mapTiles = GameManager.Instance.MapTiles;
		numRows = GameManager.Instance.NumRows;
		numCols = GameManager.Instance.NumCols;

		// get input manager and set UI to false
		inputManager = InputManager.Instance;
		orderUI.SetActive(false);

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

	// Issue orders event recieves
	void IssueOrders(IssueOrdersEvent e){
		// set state to recieving orders
		curState = UnitState.RecievingOrders;
	}

	/* 
	*******************************************
				Button Functions
	*******************************************
	*/

	// Change the active order according to what button was pressed
	void ChangeActiveOrder(int currentOrder){
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
		OrdersInteractable(true);
		CommandsInteractable(false);

		// hide UI
		orderUI.SetActive(false);

		if(command == OrderType.Hold){
			DeterminePosition(0);
		}else if(command == OrderType.Move){
			DeterminePosition(maxMove);
		}else if(command == OrderType.Attack){
			DeterminePosition(attackRange); // change this to it's own function for finding the attack area (according to attack-pattern)
		}
	}

	// highlight all tiles within range
	void DeterminePosition(int numSquares){
		// current tile position
		int curX = (int) curTile.MapPosn.y;
		int curY = (int) curTile.MapPosn.x;

		// if numSquares is 0, return current tile
		if(numSquares == 0){
			SetOrderValues(transform.position);
		}else{
			// find all tiles within range
			for(int y = 0; y < numCols; y++){
				for(int x = 0; x < numRows; x++){
					int dx = Mathf.Abs(x - curX);
					int dy = Mathf.Abs(y - curY);
					// if distance is in range, add to optional tiles
					if(dx + dy <= numSquares){
						activeTiles.Add(mapTiles[y,x]);
					}
				}
			}

			// display the tiles that can be clicked on
			ShowActiveTiles(true);
		}
	}

	// set each order's command and go-to position
	void SetOrderValues(Vector3 position){
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
			// set values of orderOne
			orderOne = activeCommand;
			orderOnePosition = position;

			// change sprite to indicate order's command
			orderOneButton.GetComponent<Image>().sprite = curSprite;
		}else if(activeOrder == 2){
			// set values of orderTwoA
			orderTwoA = activeCommand;
			orderTwoAPosition = position;

			// change sprite to indicate order's command
			orderOneButton.GetComponent<Image>().sprite = curSprite;
		}else if(activeOrder == 3){
			// set values of orderTwoB
			orderTwoB = activeCommand;
			orderTwoBPosition = position;

			// change sprite to indicate order's command
			orderOneButton.GetComponent<Image>().sprite = curSprite;
		}
	}

	// set the order interactability
	void OrdersInteractable(bool interact){
		orderOneButton.interactable = interact;
		orderTwoAButton.interactable = interact;
		orderTwoBButton.interactable = interact;
	}

	// set the command interactability
	void CommandsInteractable(bool interact){
		holdCommand.interactable = interact;
		moveCommand.interactable = interact;
		attackCommand.interactable = interact;
	}

	// light/de-light active tiles
	void ShowActiveTiles(bool show){
		for(int i = 0; i < activeTiles.Count; i++){
			activeTiles[i].GetComponent<BaseTile>().LightUp(show);
		}
	}

	void ClearActiveTiles(){
		activeOrder = 0;
		activeCommand = OrderType.Null;
		ShowActiveTiles(false);
		activeTiles.Clear();
	}

	/* 
	*******************************************
			Click Event Listener
	*******************************************
	*/

	void TileClicked(TileClickedEvent e){
		if(curState == UnitState.RecievingOrders){
			if(activeTiles.Contains(e.Tile.gameObject)){
				// if tile is active, set position of order
				SetOrderValues(e.Tile.position);

				// and clear active tiles (as one was chosen)
				ClearActiveTiles();
			}else{
				if(orderUI.activeSelf){// if UI is open, close UI
					orderUI.SetActive(false);
				}else if(activeTiles.Count != 0){// if tiles are active
					// do nothing
				}
			}
		}
	}

	void UnitClicked(UnitClickedEvent e){
		if(e.Unit == transform){
			if(curState == UnitState.Waiting){
				// nothing?
			}else if(curState == UnitState.RecievingOrders){
				// display order UI
				orderUI.SetActive(true);

				// set what can be clicked on
				OrdersInteractable(true);
				CommandsInteractable(false);
			}else if(curState == UnitState.DoingOrderOne){
				// nothing?
			}else if(curState == UnitState.ChoosingOrderTwo){
				// show orders/start process
			}else if(curState == UnitState.DoingOrderTwo){
				// nothing?
			}
		}else{
			// different unit was selected, remove clickable tiles and remove UI
			orderUI.SetActive(false);
			ClearActiveTiles();
		}
	}

	/* 
	*******************************************
			Publicly Available Variables
	*******************************************
	*/

	// tile tjat unit is currently on
	public BaseTile CurTile{
		get {return curTile;}
		set {curTile = value;}
	}

	public UnitState CurState{
		get {return curState;}
		set {curState = value;}
	}

	public OrderType OrderOne{
		get {return orderOne;}
	}

	public Vector3 OrderOnePosition{
		get {return orderOnePosition;}
	}

	public OrderType OrderTwoA{
		get {return orderTwoA;}
	}

	public Vector3 OrderTwoAPosition{
		get {return orderTwoAPosition;}
	}

	public OrderType OrderTwoB{
		get {return orderTwoB;}
	}

	public Vector3 OrderTwoBPosition{
		get {return orderTwoBPosition;}
	}
}
