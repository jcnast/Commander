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
	private bool cameraCentered = false;

	public MapManager mapManager;

	public GameObject botSide;
	private SideManager botManager;
	public GameObject leftSide;
	private SideManager leftManager;
	public GameObject topSide;
	private SideManager topManager;
	public GameObject rightSide;
	private SideManager rightManager;

	private GameStage curStage;
	private GameTurn curTurn;
	private bool inGameLoop = false;

	// Use this for initialization
	void Start () {
		botManager = botSide.GetComponent<SideManager>();
		leftManager = leftSide.GetComponent<SideManager>();
		topManager = topSide.GetComponent<SideManager>();
		rightManager = rightSide.GetComponent<SideManager>();

		curStage = GameStage.SetUp;
	}
	
	// Update is called once per frame
	void Update () {

		if(!inGameLoop){
			switch (curStage){
				case GameStage.SideSelect:
					SideSelectUpdate();
					break;
				case GameStage.UnitPlacement:
					UnitPlacementUpdate();
					break;
				case GameStage.GameLoop:
					inGameLoop = true;
					break;
				case GameStage.GameOver:
					GameOverUpdate();
					break;
			}

		}else if(inGameLoop){
			switch(curTurn){
				case GameTurn.IssueOrders:
					IssueOrderUpdate();
					break;
				case GameTurn.FirstOrder:
					FirstOrderUpdate();
					break;
				case GameTurn.OrderSelect:
					OrderSelectUpdate();
					break;
				case GameTurn.SecondOrder:
					SecondOrderUpdate();
					break;
			}
		}
	}

	private bool sideSelectStarted = false;
	void SideSelectUpdate(){

		if(!cameraCentered){
			cameraCentered = true;

			Vector3 focalPoint = (botSide.transform.position + leftSide.transform.position + topSide.transform.position + rightSide.transform.position)/4;
			mainCamera.transform.position = new Vector3(focalPoint.x, focalPoint.y, mainCamera.transform.position.z);
		}

		if(!sideSelectStarted){
			sideSelectStarted = true;

			botManager.LightAllTiles();
			leftManager.LightAllTiles();
			topManager.LightAllTiles();
			rightManager.LightAllTiles();
		}

	}

	void UnitPlacementUpdate(){

	}

	void GameOverUpdate(){

	}

	void IssueOrderUpdate(){

	}

	void FirstOrderUpdate(){

	}

	void OrderSelectUpdate(){

	}

	void SecondOrderUpdate(){

	}


	public GameStage SetStage{
		get {return curStage;}
		set {curStage = value;}
	}

	public GameTurn SetTurn{
		get {return curTurn;}
		set {curTurn = value;}
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
