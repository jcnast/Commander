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

	void OnDestroy(){
		Events.instance.RemoveListener<StartSideSelectEvent> (StartSideSelect);
		Events.instance.RemoveListener<TileClickedEvent> (TileClicked);
	}

	void Start () {
		Events.instance.AddListener<StartSideSelectEvent> (StartSideSelect);
		Events.instance.AddListener<TileClickedEvent> (TileClicked);
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

		transform.position = (sidePieces[0].transform.position + sidePieces[sidePieces.Length - 1].transform.position)/2;

		sideBaseTiles = new BaseTile[sidePieces.Length];
		for(int i = 0; i < sidePieces.Length; i++){
			sideBaseTiles[i] = sidePieces[i].GetComponent<BaseTile>();
		}

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
