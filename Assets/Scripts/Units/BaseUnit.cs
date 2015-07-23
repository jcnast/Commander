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
		Move,
		Attack,
		Hold
	}

	public enum AttackType{
		Circle,
		Line,
		Arc
	}

	// all UI components
	public GameObject orderUI;
	public Button orderOne;
	public Button orderTwoA;
	public Button orderTwoB;
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

	private List<OrderType> orders = new List<OrderType>();

	void OnDestroy(){
		Events.instance.RemoveListener<IssueOrdersEvent> (IssueOrders);
	}

	void Start(){
		Events.instance.AddListener<IssueOrdersEvent> (IssueOrders);

		// UI button delegate assignment
		orderOne.onClick.AddListener(() => {OrderOne();});
		orderTwoA.onClick.AddListener(() => {OrderTwoA();});
		orderTwoB.onClick.AddListener(() => {OrderTwoB();});
		holdCommand.onClick.AddListener(() => {HoldCommand();});
		moveCommand.onClick.AddListener(() => {MoveCommand();});
		attackCommand.onClick.AddListener(() => {AttackCommand();});

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

	void OrderOne(){
		Debug.Log("order one");
	}

	void OrderTwoA(){
		Debug.Log("order two A");
	}

	void OrderTwoB(){
		Debug.Log("order two B");
	}

	void HoldCommand(){
		Debug.Log("order hold");
	}

	void MoveCommand(){
		Debug.Log("order move");
	}

	void AttackCommand(){
		Debug.Log("order attack");
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

	public List<OrderType> Orders{
		get {return orders;}
	}
}
