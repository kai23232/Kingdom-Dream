using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;
    
    
    /// <summary>
    /// 更新房间的事件监听函数
    /// </summary>
    /// <param name="roomVector"></param>
    public void UpdateMapLayoutData(object value)
    {
        //将object转换为Vector2Int
        var roomVector = (Vector2Int)value;
        //Debug.Log($"更新房间{roomVector}");   
        //更新地图布局数据
        var mapRoomData = mapLayout.mapRoomDataList.Find(x => x.column == roomVector.x && x.row == roomVector.y);
        mapRoomData.roomState = RoomState.Visited;
        //更新同一列数据
        var mapRoomDataSameColumn = mapLayout.mapRoomDataList.FindAll(x => x.column == roomVector.x);
        foreach (var item in mapRoomDataSameColumn)
        {
            if(item.row != roomVector.y) 
            {
                item.roomState = RoomState.Locked;
            }
        }
        
        //更新连线房间
        foreach (var link in mapRoomData.LinkToList)
        {
            var linkRoomData = mapLayout.mapRoomDataList.Find(x => x.column == link.x && x.row == link.y);
            linkRoomData.roomState = RoomState.Attainable;
        }
    }
}
