using UnityEngine;
using System.Collections;

public class BaseTile : MonoBehaviour {

	public enum TileType{
		Base,
		Water,
		Forest,
		Sand
	};

	public MapManager mapManager;
	public SpriteRenderer interactableSprite;
	private InputManager inputManager;

	public TileType tileType;
	public int movementCost;
	public int attackCost;
	private Vector2 mapPosn;
	private float spriteExtents;

	private BaseUnit unitOnTile;

	private BaseTile topLeft;
	private BaseTile topMiddle;
	private BaseTile topRight;
	private BaseTile midLeft;
	private BaseTile midRight;
	private BaseTile botLeft;
	private BaseTile botMiddle;
	private BaseTile botRight;

	void Start(){
		inputManager = InputManager.Instance;

		spriteExtents = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
	}

	void Update(){
		// check if UI was clicked on, and if there is a unit on the tile
		if(!inputManager.UISelected && unitOnTile == null){
			// check if the mouse is within the sprite's bounds
			if(inputManager.WorldCursorPosition.x >= transform.position.x - spriteExtents &&
				inputManager.WorldCursorPosition.x <= transform.position.x + spriteExtents &&
				inputManager.WorldCursorPosition.y >= transform.position.y - spriteExtents &&
				inputManager.WorldCursorPosition.y <= transform.position.y + spriteExtents){
				// check if click was released
				if(inputManager.ClickUp){
					// raise TileClickedEvent
					Events.instance.Raise( new TileClickedEvent(transform));
				}
			}
		}
	}

	// light up tile
	public void LightUp(bool enabled){
		interactableSprite.enabled = enabled;
	}

	/* 
	*******************************************
			Publicly Available Variables
	*******************************************
	*/

	// map position in the grid
	public Vector2 MapPosn{
		get {return mapPosn;}
		set {mapPosn = value;}
	}

	// the current unit on the tile
	public BaseUnit UnitOnTile{
		get {return unitOnTile;}
		set {unitOnTile = value;}
	}

	// is the tile lit up?
	public bool LitUp{
		get {return interactableSprite.enabled;}
	}

	/* 
	***************************************
			Adjacent tile selectors
	***************************************
	*/

	public BaseTile TopLeft{
		get {return topLeft;}
		set {topLeft = value;}
	}

	public BaseTile TopMiddle{
		get {return topMiddle;}
		set {topMiddle = value;}
	}

	public BaseTile TopRight{
		get {return topRight;}
		set {topRight = value;}
	}

	public BaseTile MidLeft{
		get {return midLeft;}
		set {midLeft = value;}
	}

	public BaseTile MidRight{
		get {return midRight;}
		set {midRight = value;}
	}

	public BaseTile BotLeft{
		get {return botLeft;}
		set {botLeft = value;}
	}

	public BaseTile BotMiddle{
		get {return botMiddle;}
		set {botMiddle = value;}
	}

	public BaseTile BotRight{
		get {return botRight;}
		set {botRight = value;}
	}
}
