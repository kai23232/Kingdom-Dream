using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public MapConfigSO mapConfig;
    public Room roomPrefab;
    
    private float screenWidth;
    private float screenHeight;
    //列宽
    private float columnWidth;
    //边距
    public float border;
    
    private Vector3 generatePosition;
    
    //房间数据
    public List<RoomDataSO> roomDataSOList;
    
    //房间
    private List<Room> roomList = new List<Room>();
    
    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = Camera.main.aspect * screenHeight;
        
        columnWidth = (screenWidth - border * 2) / mapConfig.roomBlueprints.Count;
    }

    private void Start()
    {
        CreateMap();
    }

    public void CreateMap()
    {
        for(int i = 0;i < mapConfig.roomBlueprints.Count;i++)
        {
            //计算起始生成点
            generatePosition = new Vector3(-screenWidth / 2 + border + columnWidth / 2 + i * columnWidth, screenHeight / 2, 0);
            //计算生成个数
            int generateCount = Random.Range(mapConfig.roomBlueprints[i].min, mapConfig.roomBlueprints[i].max + 1);
            //计算行高
            float rowHeight = screenHeight / generateCount;
            Vector3 newPoint;
            for(int j = 0;j < generateCount;j++)
            {
                newPoint = new Vector3(generatePosition.x, generatePosition.y - rowHeight / 2 - rowHeight * j, 0);
                
                //左右随机偏移
                if(i == mapConfig.roomBlueprints.Count - 1)
                {
                    newPoint.x = (screenWidth/2 - border * 2);
                }
                else if (i != 0)
                {
                    newPoint.x += Random.Range(-border/2, border/2);
                }
                
                //实例化房间
                Room room = Instantiate(roomPrefab, newPoint, Quaternion.identity);
                roomList.Add(room);
                //随机选择房间类型
                RoomType roomType = mapConfig.roomBlueprints[i].roomType;
                //选择房间数据
                RoomDataSO roomDataSO = roomDataSOList[0];
                switch (roomType)
                {
                    case RoomType.MinorEnemy:
                        roomDataSO = roomDataSOList[0];
                        break;
                    case RoomType.EliteEnemy:
                        roomDataSO = roomDataSOList[1];
                        break;
                    case RoomType.Shop:
                        roomDataSO = roomDataSOList[2];
                        break;
                    case RoomType.Treasure:
                        roomDataSO = roomDataSOList[3];
                        break;
                    case RoomType.RestRoom:
                        roomDataSO = roomDataSOList[4];
                        break;
                    case RoomType.Boss:
                        roomDataSO = roomDataSOList[5];
                        break;
                }
                room.SetUpRoom(i, j, roomDataSO);
            }
        }
    }
    
    [ContextMenu("重新生成地图")]
    public void ReGenerateMap()
    {
        foreach (var room in roomList)
        {
            Destroy(room.gameObject);
        }
        roomList.Clear();
        CreateMap();
    }
}
