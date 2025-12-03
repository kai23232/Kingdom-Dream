using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    public void OnLoadRoomEvent(object data)
    {
        if (data is RoomDataSO)
        {
            RoomDataSO roomDataSO = data as RoomDataSO;
            Debug.Log($"加载房间{roomDataSO.roomType}");
        }
    }
}
