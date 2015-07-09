using UnityEngine;
using System.Collections;

public class SideManager : MonoBehaviour {

	private GameObject[] sidePieces;

	private bool centered = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!centered){
			if(sidePieces.Length > 0){
				transform.position = (sidePieces[0].transform.position + sidePieces[sidePieces.Length - 1].transform.position)/2;
				centered = true;
			}
		}
	}

	public GameObject[] SidePieces{
		get {return sidePieces;}
		set {sidePieces = value;}
	}
}
