using UnityEngine;
using System.Collections;

public class BaseUnit : MonoBehaviour {

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
}
