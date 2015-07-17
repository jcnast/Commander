using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	public enum PlayerState{
		Waiting,
		PlacingUnits,
		Commanding,
		Selecting
	}

	private PlayerState curState = PlayerState.Waiting;

	private GameObject[] sideTiles;

	public List<GameObject> unitList = new List<GameObject>();
	private List<Transform> activeUnits = new List<Transform>();

	void OnDestroy(){
		Events.instance.RemoveListener<PlaceUnitsEvent> (PlaceUnits);
		Events.instance.RemoveListener<SingleUnitPlacedEvent> (UnitPlaced);
		Events.instance.RemoveListener<TileClickedEvent> (TileClicked);
	}

	void Start(){
		Events.instance.AddListener<PlaceUnitsEvent> (PlaceUnits);
		Events.instance.AddListener<SingleUnitPlacedEvent> (UnitPlaced);
		Events.instance.AddListener<TileClickedEvent> (TileClicked);	
	}

	// what to do when units should start to be placed
	void PlaceUnits(PlaceUnitsEvent e){
		if(e.Player == gameObject){
			curState = PlayerState.PlacingUnits;
			sideTiles = e.Side.GetComponent<SideManager>().SidePieces;
		}
	}

	private int curUnit = 0;
	// what to do when a unit was placed
	void UnitPlaced(SingleUnitPlacedEvent e){
		// make sure the unit can be placed
		if(curState == PlayerState.PlacingUnits && Array.Find(sideTiles, x => x == e.Tile.gameObject)){ // the second condition is added in because the event system is out of whack
			if(curUnit < unitList.Count){
				BaseTile tile = e.Tile.GetComponent<BaseTile>();
				BaseUnit unit = unitList[curUnit].GetComponent<KnightUnit>();
				// can't spawn knight in water
				if(tile.tileType != BaseTile.TileType.Water || unit == null){
					GameObject newUnit = (GameObject) Instantiate(unitList[curUnit], e.Tile.position, Quaternion.Euler(e.Rotation.x, e.Rotation.y, e.Rotation.z));
					newUnit.transform.SetParent(transform);
					activeUnits.Add(newUnit.transform);

					curUnit++;
				}
			}
			if(curUnit == unitList.Count){
				curState = PlayerState.Waiting;
				Events.instance.Raise(new UnitsPlacedEvent(gameObject));
			}
		}
	}


	// what to do when a tile was clicked
	void TileClicked(TileClickedEvent e){
		
	}
}
