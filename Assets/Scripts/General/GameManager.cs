using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum GameStage{
		SetUp,
		SideSelect,
		UnitPlacement,
		GameLoop,
		GameOver
	}

	public enum GameTurn{
		IssueOrders,
		FirstOrder,
		OrderSelect,
		SecondOrder
	}

	static GameManager instance;

	public Camera mainCamera;

	public MapManager mapManager;

	public int numRows = 0;
	public int numCols = 0;
	// can go from 0 to 3 (represents # of special terrain tile types enabled)
	public int terComplex = 0;
	// can go from 2 to 4 (represents # players)
	public int numPlayers = 0;

	public GameObject botSide;
	private SideManager botManager;
	public GameObject leftSide;
	private SideManager leftManager;
	public GameObject topSide;
	private SideManager topManager;
	public GameObject rightSide;
	private SideManager rightManager;

	public GameObject playerObject;
	private GameObject[] players;
	private GameObject[] activeSides;
	private int curPlayer = 0;

	private GameStage curStage;
	private GameTurn curTurn;

	void OnDestroy(){
		Events.instance.RemoveListener<MapSetUpCompleteEvent> (StartSideSelect);
		Events.instance.RemoveListener<SideSelectedEvent> (SideSelected);
		Events.instance.RemoveListener<StartUnitPlacementEvent> (StartUnitPlacement);
		Events.instance.RemoveListener<UnitsPlacedEvent> (UnitsPlaced);
		Events.instance.RemoveListener<GameStartEvent> (GameStart);
	}
	
	void Awake(){
		Events.instance.AddListener<MapSetUpCompleteEvent> (StartSideSelect);
		Events.instance.AddListener<SideSelectedEvent> (SideSelected);
		Events.instance.AddListener<StartUnitPlacementEvent> (StartUnitPlacement);
		Events.instance.AddListener<UnitsPlacedEvent> (UnitsPlaced);
		Events.instance.AddListener<GameStartEvent> (GameStart);

		// set instance
		if(instance == null){
			instance = gameObject.GetComponent<GameManager>();
		}else{
			Destroy(gameObject);
		}

		// get the SideManager component for each side
		botManager = botSide.GetComponent<SideManager>();
		leftManager = leftSide.GetComponent<SideManager>();
		topManager = topSide.GetComponent<SideManager>();
		rightManager = rightSide.GetComponent<SideManager>();

		// create the players to be rotated through
		activeSides = new GameObject[numPlayers];
		players = new GameObject[numPlayers];
		for(int i = 0; i < numPlayers; i++){
			players[i] = (GameObject) Instantiate(playerObject, transform.position, Quaternion.identity);
			players[i].name = string.Format("Player_{0}", i + 1);
		}

		curStage = GameStage.SetUp;
	}

	// side select started, center the camera
	void StartSideSelect(MapSetUpCompleteEvent e){
		curStage = GameStage.SideSelect;

		botManager.SideSetUp();
		leftManager.SideSetUp();
		topManager.SideSetUp();
		rightManager.SideSetUp();

		Vector3 focalPoint = (botSide.transform.position + leftSide.transform.position + topSide.transform.position + rightSide.transform.position)/4;
		mainCamera.transform.position = new Vector3(focalPoint.x, focalPoint.y, mainCamera.transform.position.z);

		Events.instance.Raise( new StartSideSelectEvent( ));
	}

	// once a side has been selected
	void SideSelected(SideSelectedEvent e){
		// assigned side to player
		activeSides[curPlayer] = e.Side;
		// if it is the last player, move on to unit placement
		if(curPlayer == activeSides.Length - 1){
			curPlayer = 0;
			Events.instance.Raise( new StartUnitPlacementEvent());
		}else{
			curPlayer++;
		}
	}

	// unit placement has started
	void StartUnitPlacement(StartUnitPlacementEvent e){
		curStage = GameStage.UnitPlacement;
		// start unit placement for the first player
		Events.instance.Raise(new PlaceUnitsEvent(players[curPlayer], activeSides[curPlayer]));
	}

	// once all units are placed for a specific player
	void UnitsPlaced(UnitsPlacedEvent e){
		// if it was the last player, move on to the game loop
		if(curPlayer == activeSides.Length - 1){
			curPlayer = 0;
			curTurn = GameTurn.IssueOrders;
			Events.instance.Raise(new GameStartEvent());
		}else{
			curPlayer++;
			Events.instance.Raise(new PlaceUnitsEvent(players[curPlayer], activeSides[curPlayer]));
		}
	}

	void GameStart(GameStartEvent e){
		curStage = GameStage.GameLoop;

		Events.instance.Raise(new IssueOrdersEvent());
		
		curTurn = GameTurn.IssueOrders;
	}

	/* 
	*******************************************
			Publicly Available Variables
	*******************************************
	*/

	public static GameManager Instance{
		get {return instance;}
	}

	public GameStage CurrentGameStage{
		get {return curStage;}
	}

	public GameTurn CurrentGameTurn{
		get {return curTurn;}
	}

	public int NumRows{
		get {return numRows;}
	}

	public int NumCols{
		get {return numCols;}
	}

	public GameObject[,] MapTiles{
		get {return mapManager.MapTiles;}
	}

	public SideManager GetBotManager{
		get {return botManager;}
	}

	public SideManager GetLeftManager{
		get {return leftManager;}
	}

	public SideManager GetTopManager{
		get {return topManager;}
	}

	public SideManager GetRightManager{
		get {return rightManager;}
	}
}