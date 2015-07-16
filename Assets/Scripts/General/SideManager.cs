using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SideManager : MonoBehaviour {

	public enum SideState{
		None,
		SideSelect,
		UnitPlacing,
		GameLoop
	}

	private SideState curState;
	private bool selected = false;

	private GameObject[] sidePieces;
	private BaseTile[] sideBaseTiles;

	void OnDestroy(){
		Events.instance.RemoveListener<MapSetUpCompleteEvent> (SideSetUp);
		Events.instance.RemoveListener<StartSideSelectEvent> (StartSideSelect);
		Events.instance.RemoveListener<StartUnitPlacementEvent> (StartUnitPlacement);
		Events.instance.RemoveListener<PlaceUnitsEvent> (PlaceUnits);
		Events.instance.RemoveListener<TileClickedEvent> (TileClicked);
	}

	void Start () {
		Events.instance.AddListener<MapSetUpCompleteEvent> (SideSetUp);
		Events.instance.AddListener<StartSideSelectEvent> (StartSideSelect);
		Events.instance.AddListener<StartUnitPlacementEvent> (StartUnitPlacement);
		Events.instance.AddListener<PlaceUnitsEvent> (PlaceUnits);
		Events.instance.AddListener<TileClickedEvent> (TileClicked);
	}

	// what to do when a tile was clicked
	void TileClicked(TileClickedEvent e){
		// things to do during side set up (can only access the specific tiles)
		if(Array.Find(sidePieces, x => x == e.Tile)){
			// if side is being selected, send event saying one has been chosen
			if(curState == SideState.SideSelect && !selected){
				selected = true;

				Events.instance.Raise( new SideSelectedEvent(gameObject));
				LightAllTiles(false);
			// place unit on one of the tiles along the side
			}else if(curState == SideState.UnitPlacing){
				//
			}
		// things to be done if we are within the game loop
		}else if(curState == SideState.GameLoop){
			//
		}
	}

	// center the side (necessary with multiple cameras)
	void SideSetUp(MapSetUpCompleteEvent e){
		transform.position = (sidePieces[0].transform.position + sidePieces[sidePieces.Length - 1].transform.position)/2;
	}

	// side select has started
	void StartSideSelect(StartSideSelectEvent e){
		curState = SideState.SideSelect;
		// get all the BaseTile components of the tiles
		sideBaseTiles = new BaseTile[sidePieces.Length];
		for(int i = 0; i < sidePieces.Length; i++){
			sideBaseTiles[i] = sidePieces[i].GetComponent<BaseTile>();
		}
		// light up all tiles along the side
		LightAllTiles(true);
	}

	// unit placement has started
	void StartUnitPlacement(StartUnitPlacementEvent e){
		LightAllTiles(false);
	}

	// check if this side is doing unit placement
	void PlaceUnits(PlaceUnitsEvent e){
		if(e.Player == gameObject){
			LightAllTiles(true);

			curState = SideState.UnitPlacing;
		}
	}

	// light up all tiles assigned to this side
	private void LightAllTiles(bool lit){
		for(int i = 0; i < sidePieces.Length; i++){
			sideBaseTiles[i].LightUp(lit);
		}
	}

	// get all tiles associated with this side
	public GameObject[] SidePieces{
		get {return sidePieces;}
		set {sidePieces = value;}
	}
}
