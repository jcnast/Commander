using UnityEngine;
using System.Collections;

public class SideManager : MonoBehaviour {

	public enum SideState{
		None,
		SideSelect,
		UnitPlacing,
		GameLoop
	}

	private SideState curState;

	private GameObject[] sidePieces;
	private BaseTile[] sideBaseTiles;

	private bool centered = false;

	void OnDestroy(){
		Events.instance.RemoveListener<StartSideSelectEvent> (StartSideSelect);
		Events.instance.RemoveListener<TileClickedEvent> (TileClicked);
	}

	void Start () {
		Events.instance.AddListener<StartSideSelectEvent> (StartSideSelect);
		Events.instance.AddListener<TileClickedEvent> (TileClicked);
	}
	
	// Update is called once per frame
	void Update () {
		if(!centered){
			if(sidePieces.Length > 0){
				centered = true;

				transform.position = (sidePieces[0].transform.position + sidePieces[sidePieces.Length - 1].transform.position)/2;

				sideBaseTiles = new BaseTile[sidePieces.Length];
				for(int i = 0; i < sidePieces.Length; i++){
					sideBaseTiles[i] = sidePieces[i].GetComponent<BaseTile>();
				}
			}
		}
	}

	void TileClicked(GameEvent e){
		if(curState == SideState.SideSelect){
			Events.instance.Raise( new SideSelectedEvent(gameObject));
		}else if(curState == SideState.UnitPlacing){
			//
		}else if(curState == SideState.GameLoop){
			//
		}
	}

	void StartSideSelect(GameEvent e){
		curState = SideState.SideSelect;
		LightAllTiles();
	}

	private void LightAllTiles(){
		for(int i = 0; i < sidePieces.Length; i++){
			sideBaseTiles[i].LightUp(true);
		}
	}

	public GameObject[] SidePieces{
		get {return sidePieces;}
		set {sidePieces = value;}
	}
}
