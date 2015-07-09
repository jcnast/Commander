﻿using UnityEngine;
using System.Collections;

public class BaseTile : MonoBehaviour {

	public enum TileType{
		Base,
		Water,
		Forest,
		Sand
	};

	public MapManager mapManager;

	public TileType tileType;
	private Vector2 mapPosn;

	private BaseTile topLeft;
	private BaseTile topMiddle;
	private BaseTile topRight;
	private BaseTile midLeft;
	private BaseTile midRight;
	private BaseTile botLeft;
	private BaseTile botMiddle;
	private BaseTile botRight;

	void OnMouseOver(){ // On mouse over needs collider
		Debug.Log(transform.position);
	}

	public Vector2 MapPosn{
		get {return mapPosn;}
		set {mapPosn = value;}
	}

	public BaseTile TopLeft{
		get {return topLeft;}
		set {topLeft = value;}
	}

	public BaseTile TopMiddle{
		get {return topMiddle;}
		set {topMiddle = value;}
	}

	public BaseTile TopRight{
		get {return topRight;}
		set {topRight = value;}
	}

	public BaseTile MidLeft{
		get {return midLeft;}
		set {midLeft = value;}
	}

	public BaseTile MidRight{
		get {return midRight;}
		set {midRight = value;}
	}

	public BaseTile BotLeft{
		get {return botLeft;}
		set {botLeft = value;}
	}

	public BaseTile BotMiddle{
		get {return botMiddle;}
		set {botMiddle = value;}
	}

	public BaseTile BotRight{
		get {return botRight;}
		set {botRight = value;}
	}
}