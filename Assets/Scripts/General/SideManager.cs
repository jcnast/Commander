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
		Events.instance.RemoveListener<TileClickedEvent> (TileClicked);
	}

	void Start () {
		Events.instance.AddListener<MapSetUpCompleteEvent> (SideSetUp);
		Events.instance.AddListener<StartSideSelectEvent> (StartSideSelect);
		Events.instance.AddListener<StartUnitPlacementEvent> (StartUnitPlacement);
		Events.instance.AddListener<TileClickedEvent> (TileClicked);
	}

	void TileClicked(TileClickedEvent e){
		if(Array.Find(sidePieces, x => x == e.Tile)){
			if(curState == SideState.SideSelect && !selected){
				selected = true;

				Events.instance.Raise( new SideSelectedEvent(gameObject));
				LightAllTiles(false);
			}else if(curState == SideState.UnitPlacing){
				//
			}else if(curState == SideState.GameLoop){
				//
			}
		}
	}

	void SideSetUp(MapSetUpCompleteEvent e){
		transform.position = (sidePieces[0].transform.position + sidePieces[sidePieces.Length - 1].transform.position)/2;
	}

	void StartSideSelect(StartSideSelectEvent e){
		curState = SideState.SideSelect;

		sideBaseTiles = new BaseTile[sidePieces.Length];
		for(int i = 0; i < sidePieces.Length; i++){
			sideBaseTiles[i] = sidePieces[i].GetComponent<BaseTile>();
		}

		LightAllTiles(true);
	}

	void StartUnitPlacement(StartUnitPlacementEvent e){
		curState = SideState.UnitPlacing;

		LightAllTiles(false);
	}

	private void LightAllTiles(bool lit){
		for(int i = 0; i < sidePieces.Length; i++){
			sideBaseTiles[i].LightUp(lit);
		}
	}

	public GameObject[] SidePieces{
		get {return sidePieces;}
		set {sidePieces = value;}
	}
}
