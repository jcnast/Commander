using UnityEngine;
using System.Collections;

public class SideManager : MonoBehaviour {

	private GameObject[] sidePieces;
	private BaseTile[] sideBaseTiles;

	private bool centered = false;

	// Use this for initialization
	void Start () {

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

	public void LightAllTiles(){
		for(int i = 0; i < sidePieces.Length; i++){
			sideBaseTiles[i].LightUp(true);
		}
	}

	public GameObject[] SidePieces{
		get {return sidePieces;}
		set {sidePieces = value;}
	}
}
