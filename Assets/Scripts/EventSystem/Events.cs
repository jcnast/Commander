using UnityEngine;
using System.Collections;

// map done being created
public class MapSetUpCompleteEvent: GameEvent{
	
}

// start side select
public class StartSideSelectEvent: GameEvent{

}

// once a side has been selected
public class SideSelectedEvent: GameEvent{
	public GameObject Side;

	public SideSelectedEvent(GameObject side){
		Side = side;
	}
}

// start unit placement
public class StartUnitPlacementEvent: GameEvent{

}

// tell a player to place all of their units
public class PlaceUnitsEvent: GameEvent{
	public GameObject Player;
	public GameObject Side;

	public PlaceUnitsEvent(GameObject player, GameObject side){
		Player = player;
		Side = side;
	}
}

// a unit wants to be placed on a given tile
public class SingleUnitPlacedEvent: GameEvent{
	public Transform Tile;
	public Vector3 Rotation;

	public SingleUnitPlacedEvent(Transform tile, Vector3 rotation){
		Tile = tile;
		Rotation = rotation;
	}
}

// once a player has placed all of their units
public class UnitsPlacedEvent: GameEvent{
	public GameObject Player;

	public UnitsPlacedEvent(GameObject player){
		Player = player;
	}
}

// start the game loop
public class GameStartEvent: GameEvent{

}

public class IssueOrdersEvent: GameEvent{

}

// when a tile was clicked
public class TileClickedEvent: GameEvent{
	public Transform Tile;
	public BaseTile baseTile;

	public TileClickedEvent(Transform tile, BaseTile basetile){
		Tile = tile;
		baseTile = basetile;
	}
}

// when a unit was clicked
public class UnitClickedEvent: GameEvent{
	public Transform Unit;

	public UnitClickedEvent(Transform unit){
		Unit = unit;
	}
}