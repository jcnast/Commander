using UnityEngine;
using System.Collections;

public class BaseUnit : MonoBehaviour {

	private BaseTile curTile;

	public float maxHealth;
	public float maxDamage;
	public int maxMove;
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
