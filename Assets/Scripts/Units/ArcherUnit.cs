using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcherUnit : BaseUnit {
	/* 
	*******************************************
				Command Functions
	*******************************************
	*/

	protected override void DetermineAttackPosition(int Range){
		// resulting tiles to look at
		List<PathManager.PathTile> optionalTiles = new List<PathManager.PathTile>();

		// starting tiles array
		BaseTile[] startingTiles = curTile.AllAdjacentTiles();
		// tile position determined by ints
		/*
			0, 1, 2,
			3, -, 4,
			5, 6, 7
		*/

		// next tiles
		Dictionary<int, PathManager.PathTile> curTiles = new Dictionary<int, PathManager.PathTile>();

		// add starting tiles into next tile list
		for(int i = 0; i < startingTiles.Length; i++){
			if(startingTiles[i] != null && (Range - startingTiles[i].AttackCost) >= 0){
				PathManager.PathTile startPathTile = new PathManager.PathTile();
				startPathTile.PreviousTile = null;
				startPathTile.CurrentTile = startingTiles[i];
				startPathTile.MovesLeft = Range - startingTiles[i].AttackCost;

				optionalTiles.Add(startPathTile);

				curTiles.Add(i, startPathTile);
			}
		}
		
		while(curTiles.Count > 0){
			List<int> keys = new List<int>(curTiles.Keys);

			for(int j = 0; j < keys.Count; j++){
				BaseTile nextBaseTile = null;
				if(keys[j] == 0){
					nextBaseTile = curTiles[keys[j]].CurrentTile.TopLeft;
				}else if(keys[j] == 1){
					nextBaseTile = curTiles[keys[j]].CurrentTile.TopMiddle;
				}else if(keys[j] == 2){
					nextBaseTile = curTiles[keys[j]].CurrentTile.TopRight;
				}else if(keys[j] == 3){
					nextBaseTile = curTiles[keys[j]].CurrentTile.MidLeft;
				}else if(keys[j] == 4){
					nextBaseTile = curTiles[keys[j]].CurrentTile.MidRight;
				}else if(keys[j] == 5){
					nextBaseTile = curTiles[keys[j]].CurrentTile.BotLeft;
				}else if(keys[j] == 6){
					nextBaseTile = curTiles[keys[j]].CurrentTile.BotMiddle;
				}else if(keys[j] == 7){
					nextBaseTile = curTiles[keys[j]].CurrentTile.BotRight;
				}

				if(nextBaseTile != null && (curTiles[keys[j]].MovesLeft - nextBaseTile.AttackCost) >= 0){
					PathManager.PathTile newPathTile = new PathManager.PathTile();
					newPathTile.PreviousTile = curTiles[keys[j]].CurrentTile;
					newPathTile.CurrentTile = nextBaseTile;
					newPathTile.MovesLeft = curTiles[keys[j]].MovesLeft - nextBaseTile.AttackCost;

					optionalTiles.Add(newPathTile);

					curTiles[keys[j]] = newPathTile;
				}else{
					curTiles.Remove(keys[j]);
				}
			}
		}

		activePathTiles = optionalTiles;
		ShowActiveTiles(true);
	}
}
