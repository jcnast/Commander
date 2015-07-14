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

	public GameObject[] players;
	public int curPlayer = 0;

	private GameStage curStage;
	private GameTurn curTurn;
	private bool inGameLoop = false;

	void OnDestroy(){
		Events.instance.RemoveListener<MapSetUpCompleteEvent> (StartSideSelect);
		Events.instance.RemoveListener<SideSelectedEvent> (SideSelected);
	}
	
	void Start(){
		Events.instance.AddListener<MapSetUpCompleteEvent> (StartSideSelect);
		Events.instance.AddListener<SideSelectedEvent> (SideSelected);

		botManager = botSide.GetComponent<SideManager>();
		leftManager = leftSide.GetComponent<SideManager>();
		topManager = topSide.GetComponent<SideManager>();
		rightManager = rightSide.GetComponent<SideManager>();

		players = new GameObject[numPlayers];
		//for(int i = 0; i < numPlayers; i++){
		//	players[i] = new GameObject();
		//	players[i].name = string.Format("Player_{0}", i + 1);
		//}

		curStage = GameStage.SetUp;
	}

	void StartSideSelect(MapSetUpCompleteEvent e){
		curStage = GameStage.SideSelect;

		Vector3 focalPoint = (botSide.transform.position + leftSide.transform.position + topSide.transform.position + rightSide.transform.position)/4;
		mainCamera.transform.position = new Vector3(focalPoint.x, focalPoint.y, mainCamera.transform.position.z);

		Events.instance.Raise( new StartSideSelectEvent( ));
	}

	void SideSelected(SideSelectedEvent e){
		players[curPlayer] = e.Side;
		if(curPlayer == players.Length - 1){
			curPlayer = 0;
			Events.instance.Raise( new StartUnitPlacementEvent( ));
		}else{
			curPlayer++;
		}
	}

	/* 
	*******************************************
			Publicly Available Variables
	*******************************************
	*/

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
