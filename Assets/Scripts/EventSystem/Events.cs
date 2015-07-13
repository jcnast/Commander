using UnityEngine;
using System.Collections;

public class StartSideSelectEvent: GameEvent{

}

public class SideSelectedEvent: GameEvent{
	public GameObject Side;

	public SideSelectedEvent(GameObject side){
		Side = side;
	}
}

public class TileClickedEvent: GameEvent{
	public GameObject Tile;

	public TileClickedEvent(GameObject tile){
		Tile = tile;
	}
}