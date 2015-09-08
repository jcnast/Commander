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

	// Use this for initialization
	void Start () {
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

	public List<PathTile> FindMovementTiles(BaseTile startingTile, int Radius){
		// starting tiles
		BaseTile[] startingTiles = startingTile.DirectlyAdjacentTiles();

		// resulting tiles to look at
		List<PathTile> optionalTiles = new List<PathTile>();

		// tiles remaining to be checked
		List<PathTile> recentTiles = new List<PathTile>();

		// add to optionalTiles
		for(int i = 0; i < startingTiles.Length; i++){
			// make sure it is a tile
			if(startingTiles[i] != null){
				// make sure enough moves are left
				if(Radius - startingTiles[i].MovementCost >= 0){
					PathTile newPathTile = new PathTile();
					newPathTile.PreviousTile = null;
					newPathTile.CurrentTile = startingTiles[i];
					newPathTile.MovesLeft = Radius - startingTiles[i].MovementCost;

					recentTiles.Add(newPathTile);
					optionalTiles.Add(newPathTile);
				}
			}
		}

		while(recentTiles.Count != 0){
			List<PathTile> newRecentTiles = new List<PathTile>();
			for(int j = 0; j < recentTiles.Count; j++){
				if(recentTiles[j].MovesLeft != 0){
					// all tiles to be checked from the current tile
					BaseTile[] attatchedTiles = recentTiles[j].CurrentTile.DirectlyAdjacentTiles();

					for(int k = 0; k < attatchedTiles.Length; k++){
						// make sure it is a tile, and it is not the starting tile
						if(attatchedTiles[k] != startingTile && attatchedTiles[k] != null){
							// check if enough moves are left
							if(recentTiles[j].MovesLeft - attatchedTiles[k].MovementCost >= 0){
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
			}

			// switch out the recently added tiles
			recentTiles = newRecentTiles;
		}

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
