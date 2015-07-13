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

		int botSideTile = 0;
		GameObject[] botSide = new GameObject[numRows - 2];
		int leftSideTile = 0;
		GameObject[] leftSide = new GameObject[numCols - 2];
		int topSideTile = 0;
		GameObject[] topSide = new GameObject[numRows - 2];
		int rightSideTile = 0;
		GameObject[] rightSide = new GameObject[numCols - 2];

		for(int i = 0; i < numRows; i++){
			for(int j = 0; j < numCols; j++){
				BaseTile curTile = mapTiles[i,j].GetComponent<BaseTile>();
				if(i == 0){
					if(j == 0){

						curTile.TopLeft = null;
						curTile.TopMiddle = mapTiles[i + 1, j].GetComponent<BaseTile>();
						curTile.TopRight = mapTiles[i + 1, j + 1].GetComponent<BaseTile>();
						curTile.MidLeft = null;
						curTile.MidRight = mapTiles[i, j + 1].GetComponent<BaseTile>();
						curTile.BotLeft = null;
						curTile.BotMiddle = null;
						curTile.BotRight = null;
					}else if(j == numRows - 1){

						curTile.TopLeft = mapTiles[i + 1, j - 1].GetComponent<BaseTile>();
						curTile.TopMiddle = mapTiles[i + 1, j].GetComponent<BaseTile>();
						curTile.TopRight = null;
						curTile.MidLeft = mapTiles[i, j - 1].GetComponent<BaseTile>();
						curTile.MidRight = null;
						curTile.BotLeft = null;
						curTile.BotMiddle = null;
						curTile.BotRight = null;
					}else{
						botSide[botSideTile] = mapTiles[i,j];
						botSideTile++;

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
					if(j == 0){

						curTile.TopLeft = null;
						curTile.TopMiddle = null;
						curTile.TopRight = null;
						curTile.MidLeft = null;
						curTile.MidRight = mapTiles[i, j + 1].GetComponent<BaseTile>();
						curTile.BotLeft = null;
						curTile.BotMiddle = mapTiles[i - 1, j].GetComponent<BaseTile>();
						curTile.BotRight = mapTiles[i - 1, j + 1].GetComponent<BaseTile>();
					}else if(j == numRows - 1){

						curTile.TopLeft = null;
						curTile.TopMiddle = null;
						curTile.TopRight = null;
						curTile.MidLeft = mapTiles[i, j - 1].GetComponent<BaseTile>();
						curTile.MidRight = null;
						curTile.BotLeft = mapTiles[i - 1, j - 1].GetComponent<BaseTile>();
						curTile.BotMiddle = mapTiles[i - 1, j].GetComponent<BaseTile>();
						curTile.BotRight = null;
					}else{
						topSide[topSideTile] = mapTiles[i,j];
						topSideTile++;

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
						leftSide[leftSideTile] = mapTiles[i,j];
						leftSideTile++;

						curTile.TopLeft = null;
						curTile.TopMiddle = mapTiles[i + 1, j].GetComponent<BaseTile>();
						curTile.TopRight = mapTiles[i + 1, j + 1].GetComponent<BaseTile>();
						curTile.MidLeft = null;
						curTile.MidRight = mapTiles[i, j + 1].GetComponent<BaseTile>();
						curTile.BotLeft = null;
						curTile.BotMiddle = mapTiles[i - 1, j].GetComponent<BaseTile>();
						curTile.BotRight = mapTiles[i - 1, j + 1].GetComponent<BaseTile>();
					}else if(j == numRows - 1){
						rightSide[rightSideTile] = mapTiles[i,j];
						rightSideTile++;

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
