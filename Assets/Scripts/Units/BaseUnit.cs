using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BaseUnit : MonoBehaviour {

	public enum UnitState{
		Waiting,
		RecievingOrders,
		DoingOrderOne,
		ChoosingOrderTwo,
		DoingOrderTwo,
	}

	public enum OrderType{
		Hold,
		Move,
		Attack,
	}

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

	private InputManager inputManager;

	private BaseTile curTile;
	private float spriteExtents;

	public float maxHealth;
	public float maxDamage;
	public int maxMove;
	// higher initiatives go first
	public int maxInitiate;

	private UnitState curState;

	private OrderType orderOne; // 1
	private Vector3 orderOnePosition = Vector3.zero;
	private OrderType orderTwoA; // 2
	private Vector3 orderTwoAPosition = Vector3.zero;
	private OrderType orderTwoB; // 3
	private Vector3 orderTwoBPosition = Vector3.zero;
	private int activeOrder;

	void OnDestroy(){
		Events.instance.RemoveListener<IssueOrdersEvent> (IssueOrders);
	}

	void Awake(){
		Events.instance.AddListener<IssueOrdersEvent> (IssueOrders);

		// UI button delegate assignment
		orderOneButton.onClick.AddListener(() => {ChangeActiveOrder(1);});
		orderTwoAButton.onClick.AddListener(() => {ChangeActiveOrder(2);});
		orderTwoBButton.onClick.AddListener(() => {ChangeActiveOrder(3);});
		holdCommand.onClick.AddListener(() => {SetOrderCommand(OrderType.Hold);});
		moveCommand.onClick.AddListener(() => {SetOrderCommand(OrderType.Move);});
		attackCommand.onClick.AddListener(() => {SetOrderCommand(OrderType.Attack);});

		inputManager = InputManager.Instance;
		orderUI.SetActive(false);

		spriteExtents = transform.GetComponent<SpriteRenderer>().bounds.extents.x;

		curState = UnitState.Waiting;
	}

	void Update(){
		// check if UI was clicked on
		if(!inputManager.UISelected){
			// check if mouse is within cursor bounds
			if(inputManager.WorldCursorPosition.x >= transform.position.x - spriteExtents &&
				inputManager.WorldCursorPosition.x <= transform.position.x + spriteExtents &&
				inputManager.WorldCursorPosition.y >= transform.position.y - spriteExtents &&
				inputManager.WorldCursorPosition.y <= transform.position.y + spriteExtents){
				// check if click was released
				if(inputManager.ClickUp){
					if(curState == UnitState.Waiting){
						// nothing?
					}else if(curState == UnitState.RecievingOrders){
						orderUI.SetActive(true);
						holdCommand.interactable = false;
						moveCommand.interactable = false;
						attackCommand.interactable = false;
					}else if(curState == UnitState.DoingOrderOne){
						// nothing?
					}else if(curState == UnitState.ChoosingOrderTwo){
						// show orders/start process
					}else if(curState == UnitState.DoingOrderTwo){
						// nothing?
					}
				}
			}else if(inputManager.ClickUp && orderUI.activeSelf){
				orderUI.SetActive(false);
			}
		}
	}

	void IssueOrders(IssueOrdersEvent e){
		curState = UnitState.RecievingOrders;
	}

	/* 
	*******************************************
				Button Functions
	*******************************************
	*/

	void ChangeActiveOrder(int currentOrder){
		activeOrder = currentOrder;
		OrdersInteractable(false);
		CommandsInteractable(true);
		Debug.Log(currentOrder);
		// set buttons to interactable
	}

	void SetOrderCommand(OrderType command){
		if(activeOrder == 1){
			orderOne = command;
			Debug.Log(activeOrder);
		}else if(activeOrder == 2){
			orderTwoA = command;
			Debug.Log(activeOrder);
		}else if(activeOrder == 3){
			orderTwoB = command;
			Debug.Log(activeOrder);
		}
		// set buttons to non-interactable
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
