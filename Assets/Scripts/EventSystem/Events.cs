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

	public PlaceUnitsEvent(GameObject player){
		Player = player;
	}
}

// once a player has placed all of their units
public class UnitsPlacedEvent: GameEvent{
	public GameObject Side;

	public UnitsPlacedEvent(GameObject side){
		Side = side;
	}
}

// when a tile was clicked
public class TileClickedEvent: GameEvent{
	public GameObject Tile;

	public TileClickedEvent(GameObject tile){
		Tile = tile;
	}
}