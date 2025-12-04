using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "MapLayout", menuName = "Map/MapLayout")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomData> mapRoomDataList = new List<MapRoomData>();
    public List<linePos> linePosList = new List<linePos>();
}

[Serializable]
public class MapRoomData
{
    public float posX, posY;
    public int column, row;
    public RoomDataSO roomData;
    public RoomState roomState;
}

[Serializable]
public class linePos
{
    public SerializeVector3 startPos, endPos;
}


