using UnityEngine;
using System.Collections;

public class BaseUnit : MonoBehaviour {

	public enum UnitState{
		Waiting,
		RecievingOrders,
		DoingOrderOne,
		ChooseingOrderTwo,
		DoingOrderTwo,
	}

	public enum AttackType{
		Circle,
		Line,
		Arc
	}

	private BaseTile curTile;

	public float maxHealth;
	public float maxDamage;
	public int maxMove;
	// higher initiatives go first
	public int maxInitiate;

	private UnitState curState;

	void OnMouseDown(){
		Debug.Log("clicked unit");
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
}
