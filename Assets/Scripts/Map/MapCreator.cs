using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour {
	// map parameters
	private int numRows = 0;
	private int numCols = 0;
	// can go from 0 to 3 (represents # of special terrain tile types enabled)
	private int terComplex = 0;
	// possible number of special tiles
	private int maxSpecial = 0;
	private int minSpecial = 0;
	// tile options
	public GameObject[] groundTiles;
	public GameObject[] waterTiles;
	public GameObject[] forestTiles;
	public GameObject[] sandTiles;
	// map elements
	private Transform MapHolder;
	private GameObject[,] mapElements;
	// current number of special tiles
	private int waterCount = 0;
	private int forestCount = 0;
	private int sandCount = 0;

	// setting up the special tile allowed amounts
	void SetUpParameters(){
		if(terComplex != 0){
			maxSpecial = (numRows * numCols)/(3 * terComplex);
			minSpecial = (numRows * numCols)/(6 * terComplex);
		}

		MapHolder = new GameObject("Map").transform;
	}

	void CreateGrid(){
		mapElements = new GameObject[numRows, numCols];

		// odds (out of sum) to spawn the named tile
		//int default_groundChance = numRows/2;
		int default_waterChance = 1;
		int default_forestChance = 1;
		int default_sandChance = 1;
		if(terComplex == 0){
			default_waterChance = 0;
			default_forestChance = 0;
			default_sandChance = 0;
		}else if(terComplex == 1){
			int randTerrain = Random.Range(0, 3);
			if(randTerrain == 0){
				default_forestChance = 0;
				default_sandChance = 0;
			}else if(randTerrain == 1){
				default_waterChance = 0;
				default_sandChance = 0;
			}else if(randTerrain == 2){
				default_waterChance = 0;
				default_forestChance = 0;
			}
		}else if(terComplex == 2){
			int randTerrain = Random.Range(0, 3);
			if(randTerrain == 0){
				default_waterChance = 0;
			}else if(randTerrain == 1){
				default_forestChance = 0;
			}else if(randTerrain == 2){
				default_sandChance = 0;
			}
		}

		for(int i = 0; i < numRows; i++){
			//int groundChanceModifier = Mathf.Abs((numCols/2) - i)%(numCols/2);
			//if(groundChanceModifier == 0){
			//	groundChanceModifier = numCols/2;
			//}
			for(int j = 0; j < numCols; j++){
				//int init_groundChance = Mathf.Abs(default_groundChance - j)%default_groundChance;
				//if(init_groundChance == 0){
				//	init_groundChance = default_groundChance;
				//}
				//int groundChance = init_groundChance + groundChanceModifier;
				int groundChance = Mathf.Max(1, 2 * (int) Mathf.Sqrt((i - (numRows/2))*(i - (numRows/2)) + (j - (numCols/2))*(j - (numCols/2))));
				int waterChance = default_waterChance;
				int forestChance = default_forestChance;
				int sandChance = default_sandChance;
				
				GameObject bottomTile = null;
				GameObject leftTile = null;
				if(i != 0){
					bottomTile = mapElements[i - 1, j];
				}
				if(j != 0){
					leftTile = mapElements[i, j - 1];
				}

				string botTag;
				string leftTag;
				if(bottomTile != null && leftTile != null){
					botTag = bottomTile.tag;
					leftTag = leftTile.tag;

					if(botTag == leftTag){
						if(botTag == "Water"){
							if(waterCount < minSpecial){
								groundChance = 0;
								forestChance = 0;
								sandChance = 0;
							}else if(waterCount < maxSpecial){
								waterChance += groundChance + forestChance + sandChance;
							}
						}else if(botTag == "Forest"){
							if(forestCount < minSpecial){
								groundChance = 0;
								waterChance = 0;
								sandChance = 0;
							}else if(forestCount < maxSpecial){
								forestChance += groundChance + waterChance + sandChance;
							}
						}else if(botTag == "Sand"){
							if(sandCount < minSpecial){
								groundChance = 0;
								waterChance = 0;
								forestChance = 0;
							}else if(sandCount < maxSpecial){
								sandChance += groundChance + waterChance + forestChance;
							}
						}
					}else{
						string curTag;
						if(botTag == "Ground"){
							curTag = leftTag;
						}else if(leftTag == "Ground"){
							curTag = botTag;
						}else{
							int randomTag = Random.Range(0,2);
							if(randomTag == 0){
								curTag = botTag;
							}else{
								curTag = leftTag;
							}
						}

						if(curTag == "Water"){
							if(waterCount < minSpecial){
								waterChance += groundChance + forestChance + sandChance;
							}
						}else if(leftTag == "Forest"){
							if(forestCount < minSpecial){
								forestChance += groundChance + waterChance + sandChance;
							}
						}else if(leftTag == "Sand"){
							if(sandCount < minSpecial){
								sandChance += groundChance + waterChance + forestChance;
							}
						}
					}
				}else if(bottomTile != null){
					botTag = bottomTile.tag;

					if(botTag == "Water"){
						if(waterCount < minSpecial){
							groundChance = 0;
							forestChance = 0;
							sandChance = 0;
						}
					}else if(botTag == "Forest"){
						if(forestCount < minSpecial){
							groundChance = 0;
							waterChance = 0;
							sandChance = 0;
						}
					}else if(botTag == "Sand"){
						if(sandCount < minSpecial){
							groundChance = 0;
							waterChance = 0;
							forestChance = 0;
						}
					}
				}else if(leftTile != null){
					leftTag = leftTile.tag;

					if(leftTag == "Water"){
						if(waterCount < minSpecial){
							waterChance += groundChance + forestChance + sandChance;
						}
					}else if(leftTag == "Forest"){
						if(forestCount < minSpecial){
							forestChance += groundChance + waterChance + sandChance;
						}
					}else if(leftTag == "Sand"){
						if(sandCount < minSpecial){
							sandChance += groundChance + waterChance + forestChance;
						}
					}
				}else{
					//waterChance = 0;
					//forestChance = 0;
					//sandChance = 0;
				}

				if(waterCount >= maxSpecial){
					waterChance = 0;
				}
				if(forestCount >= maxSpecial){
					forestChance = 0;
				}
				if(sandCount >= maxSpecial){
					sandChance = 0;
				}

				int randomTile = Random.Range(0, groundChance + waterChance + forestChance + sandChance);
				if(randomTile < groundChance){
					mapElements[i, j] = groundTiles[Random.Range(0, groundTiles.Length)];
				}else if(randomTile < groundChance + waterChance){
					waterCount++;
					mapElements[i, j] = waterTiles[Random.Range(0, waterTiles.Length)];
				}else if(randomTile < groundChance + waterChance + forestChance){
					forestCount++;
					mapElements[i, j] = forestTiles[Random.Range(0, forestTiles.Length)];
				}else if(randomTile < groundChance + waterChance + forestChance + sandChance){
					sandCount++;
					mapElements[i, j] = sandTiles[Random.Range(0, sandTiles.Length)];
				}
			}
		}
	}

	void SpawnTiles(){
		Vector3 curPosn = Vector3.zero;
		for(int i = 0; i < numRows; i++){
			for(int j = 0; j < numCols; j++){
				GameObject curElement = (GameObject) Instantiate(mapElements[i, j], curPosn, Quaternion.identity);

				BaseTile curTile = curElement.GetComponent<BaseTile>();
				curTile.MapPosn = new Vector2(i, j);

				curElement.transform.SetParent(MapHolder, true);
				curPosn += new Vector3(1, 0, 0);
			}
			curPosn = new Vector3(0, curPosn.y + 1, 0);
		}
	}

	public void CreateMap(int rows, int cols, int complexity){
		numRows = rows;
		numCols = cols;
		terComplex = complexity;
		SetUpParameters();
		CreateGrid();
		SpawnTiles();
	}

	public GameObject[,] getMapElements{
		get {return mapElements;}
	}
}
