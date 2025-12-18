using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;


    public List<Enemy> aliveEnemtList = new List<Enemy>();
    
    [Header("广播")]
    public ObjectEventSO gameWinEvent;

    public ObjectEventSO gameOverEvent;
    

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
        
        aliveEnemtList.Clear();
    }
    
    public void OnRoomLoadedEvent(object data)
    {
        var enemys = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemys)
        {
            aliveEnemtList.Add(enemy);
        }
    }
    
    public void OnCharacterDeadEvent(object character)
    {
        if (character is Player)
        {
            //发出失败的通知
            StartCoroutine(EventDelayAction(gameOverEvent));
        }

        if (character is Enemy)
        {
            aliveEnemtList.Remove(character as Enemy);
            //检查是否所有敌人都死亡
            if (aliveEnemtList.Count == 0)
            {
                //发出胜利通知
                StartCoroutine(EventDelayAction(gameWinEvent));
            }
        }

        if (character is Boss)
        {
            //发出失败的通知
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
    }

    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO.RaiseEvent(null,this);
    }
    
    public void MapClear()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePosList.Clear(); 
    }
}
