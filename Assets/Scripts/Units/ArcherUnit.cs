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

		// current tiles list
		Dictionary<int, BaseTile> curTiles = new Dictionary<int, BaseTile>();

		// next tiles
		Dictionary<int, BaseTile> nextTiles = new Dictionary<int, BaseTile>();

		// add starting tiles into next tile list
		for(int i = 0; i < startingTiles.Length; i++){
			nextTiles.Add(startingTiles[i], i);
		}
		
		while(nextTiles.Count > 0){
			List<int> keys = new List<int>(this.nextTiles.Keys);

			for(int j = 0; j < keys.Count; j++){
				PathManager.PathTile curPathTile = new PathManager.PathTile();
				curPathTile.PreviousTile = curTiles[keys[j]];
				curPathTile.CurrentTile = nextTiles[key[j]];
			}
		}
	}
}
