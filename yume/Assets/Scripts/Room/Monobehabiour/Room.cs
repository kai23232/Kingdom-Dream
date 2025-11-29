using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //坐标
    public int column;

    public int line;
    //房间渲染器
    private SpriteRenderer spriteRenderer;
    //房间数据
    public RoomDataSO roomDataSO;
    // 房间状态
    public RoomState roomState;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        SetUpRoom(0,0, roomDataSO);
    }

    private void OnMouseDown()
    {
        //处理点击事件
        Debug.Log($"点击了房间{roomDataSO.roomType}");
    }
    
    /// <summary>
    /// 外部创建房间时调用配置房间
    /// </summary>
    /// <param name="column"></param>
    /// <param name="line"></param>
    /// <param name="roomDataSO"></param>
    public void SetUpRoom(int column, int line,RoomDataSO roomDataSO)
    {
        this.column = column;
        this.line = line;
        this.roomDataSO = roomDataSO;
        
        spriteRenderer.sprite = roomDataSO.roomIcon;
    }
}
