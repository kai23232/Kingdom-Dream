using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
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
    }
    
    public void PlayerTurnBegin()
    {
        timeCounter = 0f;
        isPlayerTurn = true;
        isEnemyTurn = false;
        playerTurnBeginEvent.RaiseEvent(null,this);
    }
    
    public void EnemyTurnBegin()
    {
        timeCounter = 0f;
        isEnemyTurn = true;
        isPlayerTurn = false;
    }
}
