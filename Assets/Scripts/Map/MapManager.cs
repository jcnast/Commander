using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

	public MapCreator mapCreator;
	public GameManager gameManager;

	public int numRows = 0;
	public int numCols = 0;
	// can go from 0 to 3 (represents # of special terrain tile types enabled)
	public int terComplex = 0;

	private GameObject[,] mapTiles;

	void Start () {
		mapCreator.CreateMap(numRows, numCols, terComplex);
		mapTiles = mapCreator.getMapElements;

		GameObject[] botSide = new GameObject[numRows];
		GameObject[] leftSide = new GameObject[numCols];
		GameObject[] topSide = new GameObject[numRows];
		GameObject[] rightSide = new GameObject[numCols];

		for(int i = 0; i < numRows; i++){
			for(int j = 0; j < numCols; j++){
				BaseTile curTile = mapTiles[i,j].GetComponent<BaseTile>();
				if(i == 0){
					botSide[j] = mapTiles[i,j];
					if(j == 0){
						leftSide[i] = mapTiles[i,j];

						curTile.TopLeft = null;
						curTile.TopMiddle = mapTiles[i + 1, j].GetComponent<BaseTile>();
						curTile.TopRight = mapTiles[i + 1, j + 1].GetComponent<BaseTile>();
						curTile.MidLeft = null;
						curTile.MidRight = mapTiles[i, j + 1].GetComponent<BaseTile>();
						curTile.BotLeft = null;
						curTile.BotMiddle = null;
						curTile.BotRight = null;
					}else if(j == numRows - 1){
						rightSide[i] = mapTiles[i,j];

						curTile.TopLeft = mapTiles[i + 1, j - 1].GetComponent<BaseTile>();
						curTile.TopMiddle = mapTiles[i + 1, j].GetComponent<BaseTile>();
						curTile.TopRight = null;
						curTile.MidLeft = mapTiles[i, j - 1].GetComponent<BaseTile>();
						curTile.MidRight = null;
						curTile.BotLeft = null;
						curTile.BotMiddle = null;
						curTile.BotRight = null;
					}else{

						curTile.TopLeft = mapTiles[i + 1, j - 1].GetComponent<BaseTile>();
						curTile.TopMiddle = mapTiles[i + 1, j].GetComponent<BaseTile>();
						curTile.TopRight = mapTiles[i + 1, j + 1].GetComponent<BaseTile>();
						curTile.MidLeft = mapTiles[i, j - 1].GetComponent<BaseTile>();
						curTile.MidRight = mapTiles[i, j + 1].GetComponent<BaseTile>();
						curTile.BotLeft = null;
						curTile.BotMiddle = null;
						curTile.BotRight = null;
					}
				}else if(i == numRows - 1){
					topSide[j] = mapTiles[i,j];
					if(j == 0){
						leftSide[i] = mapTiles[i,j];

						curTile.TopLeft = null;
						curTile.TopMiddle = null;
						curTile.TopRight = null;
						curTile.MidLeft = null;
						curTile.MidRight = mapTiles[i, j + 1].GetComponent<BaseTile>();
						curTile.BotLeft = null;
						curTile.BotMiddle = mapTiles[i - 1, j].GetComponent<BaseTile>();
						curTile.BotRight = mapTiles[i - 1, j + 1].GetComponent<BaseTile>();
					}else if(j == numRows - 1){
						rightSide[i] = mapTiles[i,j];

						curTile.TopLeft = null;
						curTile.TopMiddle = null;
						curTile.TopRight = null;
						curTile.MidLeft = mapTiles[i, j - 1].GetComponent<BaseTile>();
						curTile.MidRight = null;
						curTile.BotLeft = mapTiles[i - 1, j - 1].GetComponent<BaseTile>();
						curTile.BotMiddle = mapTiles[i - 1, j].GetComponent<BaseTile>();
						curTile.BotRight = null;
					}else{

						curTile.TopLeft = null;
						curTile.TopMiddle = null;
						curTile.TopRight = null;
						curTile.MidLeft = mapTiles[i, j - 1].GetComponent<BaseTile>();
						curTile.MidRight = mapTiles[i, j + 1].GetComponent<BaseTile>();
						curTile.BotLeft = mapTiles[i - 1, j - 1].GetComponent<BaseTile>();
						curTile.BotMiddle = mapTiles[i - 1, j].GetComponent<BaseTile>();
						curTile.BotRight = mapTiles[i - 1, j + 1].GetComponent<BaseTile>();
					}
				}else{
					if(j == 0){
						leftSide[i] = mapTiles[i,j];

						curTile.TopLeft = null;
						curTile.TopMiddle = mapTiles[i + 1, j].GetComponent<BaseTile>();
						curTile.TopRight = mapTiles[i + 1, j + 1].GetComponent<BaseTile>();
						curTile.MidLeft = null;
						curTile.MidRight = mapTiles[i, j + 1].GetComponent<BaseTile>();
						curTile.BotLeft = null;
						curTile.BotMiddle = mapTiles[i - 1, j].GetComponent<BaseTile>();
						curTile.BotRight = mapTiles[i - 1, j + 1].GetComponent<BaseTile>();
					}else if(j == numRows - 1){
						rightSide[i] = mapTiles[i,j];

						curTile.TopLeft = mapTiles[i + 1, j - 1].GetComponent<BaseTile>();
						curTile.TopMiddle = mapTiles[i + 1, j].GetComponent<BaseTile>();
						curTile.TopRight = null;
						curTile.MidLeft = mapTiles[i, j - 1].GetComponent<BaseTile>();
						curTile.MidRight = null;
						curTile.BotLeft = mapTiles[i - 1, j - 1].GetComponent<BaseTile>();
						curTile.BotMiddle = mapTiles[i - 1, j].GetComponent<BaseTile>();
						curTile.BotRight = null;
					}else{

						curTile.TopLeft = mapTiles[i + 1, j - 1].GetComponent<BaseTile>();
						curTile.TopMiddle = mapTiles[i + 1, j].GetComponent<BaseTile>();
						curTile.TopRight = mapTiles[i + 1, j + 1].GetComponent<BaseTile>();
						curTile.MidLeft = mapTiles[i, j - 1].GetComponent<BaseTile>();
						curTile.MidRight = mapTiles[i, j + 1].GetComponent<BaseTile>();
						curTile.BotLeft = mapTiles[i - 1, j - 1].GetComponent<BaseTile>();
						curTile.BotMiddle = mapTiles[i - 1, j].GetComponent<BaseTile>();
						curTile.BotRight = mapTiles[i - 1, j + 1].GetComponent<BaseTile>();
					}
				}
			}
		}

		gameManager.GetBotManager.SidePieces = botSide;
		gameManager.GetLeftManager.SidePieces = leftSide;
		gameManager.GetTopManager.SidePieces = topSide;
		gameManager.GetRightManager.SidePieces = rightSide;

		gameManager.SetStage = GameManager.GameStage.SideSelect;
	}

	public GameObject[,] MapTiles{
		get {return mapTiles;}
	}
}
