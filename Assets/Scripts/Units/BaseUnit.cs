using UnityEngine;
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

	public enum AttackType{
		Circle,
		Line,
		Arc
	}

	public enum OrderType{
		Move,
		Attack,
		Hold
	}

	public GameObject orderUI;

	private BaseTile curTile;

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

	void Awake(){
		Events.instance.AddListener<IssueOrdersEvent> (IssueOrders);
		orderUI.SetActive(false);

		curState = UnitState.Waiting;
	}

	void OnMouseDown(){
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

	void IssueOrders(IssueOrdersEvent e){
		curState = UnitState.RecievingOrders;
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
