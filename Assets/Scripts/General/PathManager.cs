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
	public class PathTile {
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
	public List<PathTile> FindOptionalTiles(PathType pathType, BaseUnit.AttackType attackType, BaseTile startingTile, int pathLength){

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
		List<PathTile> recentTiles = new List<PathTile>();

		// add to optionalTiles
		for(int i = 0; i < startingTiles.Length; i++){
			if(startingTiles[i] != null){
				PathTile newPathTile = new PathTile();
				newPathTile.PreviousTile = null;
				newPathTile.CurrentTile = startingTiles[i];
				newPathTile.MovesLeft = Radius - startingTiles[i].MovementCost;

				recentTiles.Add(newPathTile);
				optionalTiles.Add(newPathTile);
			}
		}

		while(recentTiles.Count != 0){
			List<PathTile> newRecentTiles = new List<PathTile>();
			for(int j = 0; j < recentTiles.Count; j++){
				if(recentTiles[j].MovesLeft != 0){
					// all tiles to be checked from the current tile
					BaseTile[] attatchedTiles = recentTiles[j].CurrentTile.DirectlyAdjacentTiles();

					for(int k = 0; k < attatchedTiles.Length; k++){
						if(attatchedTiles[k] != startingTile && attatchedTiles[k] != null){
							// create PathTile, add to optionaltiles if within range
							PathTile newPathTile = new PathTile();
							newPathTile.PreviousTile = recentTiles[j].CurrentTile;
							newPathTile.CurrentTile = attatchedTiles[k];
							newPathTile.MovesLeft = recentTiles[j].MovesLeft - attatchedTiles[k].MovementCost;

							// if the current tile already is in the optional tiles and has more steps remaining replace the existing tile
							bool alreadyIn = false;
							for(int l = 0; l < optionalTiles.Count; l++){
								if(optionalTiles[l].CurrentTile == newPathTile.CurrentTile){
									if(optionalTiles[l].MovesLeft <= newPathTile.MovesLeft){
										optionalTiles.Remove(optionalTiles[l]);
										optionalTiles.Add(newPathTile);

										newRecentTiles.Add(newPathTile);

										alreadyIn = true;
										break;
									}
								}
							}

							// otherwise, just add the tile
							if(!alreadyIn){
								optionalTiles.Add(newPathTile);

								newRecentTiles.Add(newPathTile);
							}
						}
					}
				}
			}

			// switch out the recently added tiles
			recentTiles = newRecentTiles;
		}

		return optionalTiles;
	}

	//	Attack path functions
	
	public List<PathTile> FindTilesInCircle(BaseTile startingTile, int Radius){
		// resulting tiles to look at
		List<PathTile> optionalTiles = new List<PathTile>();

		return optionalTiles;
	}

	public List<PathTile> FindTilesInLine(BaseTile startingTile, int Length){
		// resulting tiles to look at
		List<PathTile> optionalTiles = new List<PathTile>();

		return optionalTiles;
	}

	public List<PathTile> FindTilesInArc(BaseTile startingTile, int Arc){
		// resulting tiles to look at
		List<PathTile> optionalTiles = new List<PathTile>();

		return optionalTiles;
	}
	
	/* 
	*******************************************
			Publicly Available Variables
	*******************************************
	*/

	public static PathManager Instance{
		get {return instance;}
	}
}
