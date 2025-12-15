using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    public GameObject playerObj;
    
    private bool isPlayerTurn = false;
    private bool isEnemyTurn = false;
    public bool battleEnd;

    private float timeCounter;
    public float energyTurnDuration;
    public float playerTurnDuration;
    
    [Header("广播")]
    public ObjectEventSO playerTurnBeginEvent;
    public ObjectEventSO enemyTurnBeginEvent;

    private void Update()
    {
        if (battleEnd)
            return;

        if (isEnemyTurn)
        {
            timeCounter += Time.deltaTime;
            if(timeCounter >= energyTurnDuration)
            {
                //玩家回合开始
                PlayerTurnBegin();
            }
        }

        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;
            if(timeCounter >= playerTurnDuration)
            {
                //敌人回合开始
                EnemyTurnBegin();
            }
        }
    }
    [ContextMenu("测试游戏开始")]
    public void GameStart()
    {
        isPlayerTurn = true;
        isEnemyTurn = false;
        battleEnd = false;
        timeCounter = 0;
        playerTurnBeginEvent.RaiseEvent(null,this);
    }
    
    public void PlayerTurnBegin()
    {
        timeCounter = 0f;
        isPlayerTurn = true;
        isEnemyTurn = false;
        playerTurnBeginEvent.RaiseEvent(null,this);
    }
    
    private void EnemyTurnBegin()
    {
        enemyTurnBeginEvent.RaiseEvent(null,this);
    }
    
    /// <summary>
    /// 监听事件函数
    /// </summary>
    public void EnemyTurnBeginListen()
    {
        timeCounter = 0f;
        isEnemyTurn = true;
        isPlayerTurn = false;
    }
    
    /// <summary>
    /// 注册时间函数 after room load
    /// </summary>
    /// <param name="data"></param>
    public void OnRoomLoadedEvent(object data)
    {
        Room currentRoom = data as Room;
        switch (currentRoom.roomDataSO.roomType)
        {
            case RoomType.EliteEnemy:
            case RoomType.MinorEnemy:
            case RoomType.Boss:
                playerObj.SetActive(true);
                GameStart();
                break;
            case RoomType.Shop:
                playerObj.SetActive(false);
                break;
            case RoomType.Treasure:
                playerObj.SetActive(false);
                break;
            case RoomType.RestRoom:
                playerObj.SetActive(true);
                break;
        }
    }
}
