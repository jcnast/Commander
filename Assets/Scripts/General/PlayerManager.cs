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
	private List<Transform> orderedOutUnits = new List<Transform>();

	void OnDestroy(){
		Events.instance.RemoveListener<PlaceUnitsEvent> (PlaceUnits);
		Events.instance.RemoveListener<SingleUnitPlacedEvent> (UnitPlaced);
		Events.instance.RemoveListener<IssueOrdersEvent> (IssueOrders);
		Events.instance.RemoveListener<TileClickedEvent> (TileClicked);
		Events.instance.RemoveListener<UnitClickedEvent> (UnitClicked);
		Events.instance.RemoveListener<UnitOrderedOutEvent> (UnitOrderedOut);
		Events.instance.RemoveListener<UnitOrderedInEvent> (UnitOrderedIn);
	}

	void Awake(){
		Events.instance.AddListener<PlaceUnitsEvent> (PlaceUnits);
		Events.instance.AddListener<SingleUnitPlacedEvent> (UnitPlaced);
		Events.instance.AddListener<IssueOrdersEvent> (IssueOrders);
		Events.instance.AddListener<TileClickedEvent> (TileClicked);
		Events.instance.AddListener<UnitClickedEvent> (UnitClicked);	
		Events.instance.AddListener<UnitOrderedOutEvent> (UnitOrderedOut);
		Events.instance.AddListener<UnitOrderedInEvent> (UnitOrderedIn);
	}

	/* 
	*******************************************
				Event Functions
	*******************************************
	*/

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
				// can't spawn two units on one tile
				if(tile.UnitOnTile == null){
					// can't spawn knight in water
					if(tile.tileType != BaseTile.TileType.Water || unit == null){
						GameObject newUnit = (GameObject) Instantiate(unitList[curUnit], e.Tile.position, Quaternion.Euler(e.Rotation.x, e.Rotation.y, e.Rotation.z));
						BaseUnit newBaseUnit = newUnit.GetComponent<BaseUnit>();

						newUnit.transform.SetParent(transform);
						activeUnits.Add(newUnit.transform);
						// assignt the respective tile/unit variables
						tile.UnitOnTile = newBaseUnit;
						newBaseUnit.CurTile = tile;

						curUnit++;
					}
				}
			}
			if(curUnit == unitList.Count){
				curState = PlayerState.Waiting;
				Events.instance.Raise(new UnitsPlacedEvent(gameObject));
			}
		}
	}

	// start issuing orders to units
	void IssueOrders(IssueOrdersEvent e){
		curState = PlayerState.Commanding;
	}

	// a unit has been ordered out
	void UnitOrderedOut(UnitOrderedOutEvent e){
		orderedOutUnits.Add(e.Unit);

		if(AllUnitsOrderedOut()){
			Events.instance.Raise(new AllUnitsOrderedOutEvent(true));
		}
	}

	void UnitOrderedIn(UnitOrderedInEvent e){
		orderedOutUnits.Remove(e.Unit);

		Events.instance.Raise(new AllUnitsOrderedOutEvent(false));
	}

	// what to do when a tile was clicked
	void TileClicked(TileClickedEvent e){
		
	}

	// what to do when a unit is clicked
	void UnitClicked(UnitClickedEvent e){

	}

	/* 
	*******************************************
				Assist Functions
	*******************************************
	*/

	public bool AllUnitsOrderedOut(){
		// if all units have been ordered out
		bool allOrderedOut = true;

		// check each unit
		for(int i = 0; i < activeUnits.Count; i++){
			bool unitOrderedOut = false;
			for(int j = 0; j < orderedOutUnits.Count; j++){
				if(activeUnits[i] == orderedOutUnits[j]){
					unitOrderedOut = true;
					break;
				}
			}

			// update allOrderedOut if accordingly
			allOrderedOut = unitOrderedOut;

			if(!unitOrderedOut){
				break;
			}
		}

		return allOrderedOut;
	}
}
