using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager : MonoBehaviour {

	// the types of possible paths
	public enum PathType{
		Movement,
		Attack
	}

	[System.Serializable]
	// to keep track of the tiles along the path to a specific point
	public class PathTile : MonoBehaviour {
		// parameters of the PathTile
		private BaseTile previousTile;
		private BaseTile currentTile;
		private int movesLeft;

		public BaseTile PreviousTile{
			get {return previousTile;}
			set {previousTile = value;}
		}

		public BaseTile CurrentTile{
			get {return currentTile;}
			set {currentTile = value;}
		}

		public int MovesLeft{
			get {return movesLeft;}
			set {movesLeft = value;}
		}
	}

	static PathManager instance;

	// info about the map
	private int numRows;
	private int numCols;
	private GameObject[,] mapTiles;

	// Use this for initialization
	void Start () {
		mapTiles = GameManager.Instance.MapTiles;
		numRows = GameManager.Instance.NumRows;
		numCols = GameManager.Instance.NumCols;

		// set instance
		if(instance == null){
			instance = gameObject.GetComponent<PathManager>();
		}else{
			Destroy(gameObject);
		}
	}
	
	/* 
	*******************************************
			Path Finding Functions
	*******************************************
	*/

	// determine the path to take from the given tile information
	public List<PathTile> FindOptionalTiles(PathType pathType, BaseUnit.AttackType attackType, int xPos, int yPos, int pathLength){
		// tile to start search from
		BaseTile startingTile = mapTiles[yPos, xPos].GetComponent<BaseTile>();

		if(pathType == PathType.Movement){
			return FindMovementTiles(startingTile, pathLength);
		}else if(pathType == PathType.Attack){
			if(attackType == BaseUnit.AttackType.Circle){
				return FindTilesInCircle(startingTile, pathLength);
			}else if(attackType == BaseUnit.AttackType.Line){
				return FindTilesInLine(startingTile, pathLength);
			}else if(attackType == BaseUnit.AttackType.Arc){
				return FindTilesInArc(startingTile, pathLength);
			}else{
				return new List<PathTile>();
			}
		}else{
			return new List<PathTile>();
		}
	}

	public List<PathTile> FindMovementTiles(BaseTile startingTile, int Radius){
		// starting tiles
		BaseTile[] startingTiles = startingTile.DirectlyAdjacentTiles();

		// resulting tiles to look at
		List<PathTile> optionalTiles = new List<PathTile>();

		// tiles remaining to be checked
		List<BaseTile> tilesToCheck = new List<BaseTile>();

		// add to optionalTiles
		for(int i = 0; i < startingtiles.Length; i++){
			tilesToCheck.Add(startingTiles[i]);
		}

		while(tilestoCheck.Count != 0){
			List<BaseTile> newTilesToCheck = new List<BaseTile>();
			for(int j = 0; j < tilesToCheck.Count; j++){
				// create PathTile, add to optionaltiles if within range

				// find surrounding tiles, add to newTilestoCheck if not in already,
				// or if the current tile has more steps remaining replace the existing tile

				// remove current tile from optionalTiles
			}
		}
	}

	//	Attack path functions
	/*
	public List<PathTile> FindTilesInCircle(BaseTile startingTile, int Radius){
		// resulting tiles to look at
		List<PathTile> optionalTiles = new List<PathTile>();
	}

	public List<PathTile> FindTilesInLine(BaseTile startingTile, int Length){
		// resulting tiles to look at
		List<PathTile> optionalTiles = new List<PathTile>();
	}

	public List<PathTile> FindTilesInArc(BaseTile startingTile, int Arc){
		// resulting tiles to look at
		List<PathTile> optionalTiles = new List<PathTile>();
	}
	*/
	/* 
	*******************************************
			Publicly Available Variables
	*******************************************
	*/

	public static PathManager Instance{
		get {return instance;}
	}
}
