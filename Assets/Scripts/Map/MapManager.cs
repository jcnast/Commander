using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

	public MapCreator mapCreator;

	public int numRows = 0;
	public int numCols = 0;
	// can go from 0 to 3 (represents # of special terrain tile types enabled)
	public int terComplex = 0;

	private GameObject[,] mapTiles;

	void Start () {
		mapCreator.CreateMap(numRows, numCols, terComplex);
		mapTiles = mapCreator.getMapElements;
		for(int i = 0; i < numRows; i++){
			for(int j = 0; j < numCols; j++){
				Debug.Log(mapTiles[i,j].GetComponent<BaseTile>().tileType);
				Debug.Log(mapTiles[i,j].GetComponent<BaseTile>().MapPosn.x);
			}
		}
	}

	public GameObject[,] MapTiles{
		get {return mapTiles;}
	}
}
