using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [Header("地图配置表")]
    public MapConfigSO mapConfig;
    
    [Header("地图布局")]
    public MapLayoutSO mapLayout;
    
    [Header("预制体")]
    public Room roomPrefab;
    public LineRenderer linePrefab;
    
    private float screenWidth;
    private float screenHeight;
    //列宽
    private float columnWidth;
    //边距
    public float border;
    
    private Vector3 generatePosition;
    
    //房间数据
    public List<RoomDataSO> roomDataSOList;
    private Dictionary<RoomType,RoomDataSO> roomDataDic = new Dictionary<RoomType, RoomDataSO>();
    
    //房间
    private List<Room> roomList = new List<Room>();
    //连线
    private List<LineRenderer> lineList = new List<LineRenderer>();
    
    
    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = Camera.main.aspect * screenHeight;
        
        columnWidth = (screenWidth - border * 2) / mapConfig.roomBlueprints.Count;
        
        //初始化房间数据字典
        foreach (var roomDataSO in roomDataSOList)
        {
            roomDataDic.Add(roomDataSO.roomType, roomDataSO);
        }
    }

    // private void Start()
    // {
    //     CreateMap();
    // }

    private void OnEnable()
    {
        if (mapLayout.mapRoomDataList.Count > 0)
        {
            LoadMap();
        }
        else
        {
            CreateMap();
        }
    }

    public void CreateMap()
    {
        //创建前一列房间列表
        List<Room> previousRoomList = new List<Room>();
        
        for(int i = 0;i < mapConfig.roomBlueprints.Count;i++)
        {
            //计算起始生成点
            generatePosition = new Vector3(-screenWidth / 2 + border + columnWidth / 2 + i * columnWidth, screenHeight / 2, 0);
            //计算生成个数
            int generateCount = Random.Range(mapConfig.roomBlueprints[i].min, mapConfig.roomBlueprints[i].max + 1);
            //计算行高
            float rowHeight = screenHeight / generateCount;
            Vector3 newPoint;
            
            //当前列房间列表
            List<Room> currentRoomList = new List<Room>();
            
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
                Room room = Instantiate(roomPrefab, newPoint, Quaternion.identity,transform);
                roomList.Add(room);
                currentRoomList.Add(room);
                //随机选择房间类型
                RoomType flags = GetRandomRoomType(mapConfig.roomBlueprints[i].roomType);
                //设置房间数据
                RoomDataSO roomDataSO = GetRoomData(flags);
                room.SetUpRoom(i, j, roomDataSO);
            }
            
            //连接前一列房间
            if(previousRoomList.Count > 0)
            {
                CreateConnections(previousRoomList, currentRoomList);
            }
            
            //更新前一列房间列表
            previousRoomList = currentRoomList;
        }
        
        SaveMap();
    }

    private void CreateConnections(List<Room> column1, List<Room> column2)
    {
        //第二列中已经连接的房间
        HashSet<Room> connectedRooms = new HashSet<Room>();
        foreach (var room in column1)
        {
            Room connectedRoom = ConnectToRandomRoom(room, column2);
            connectedRooms.Add(connectedRoom);
        }
        
        foreach (var room in column2)
        {
            if(!connectedRooms.Contains(room))
            {
                ConnectFromRandomRoom(room, column1);
            }
        }
    }

    private Room ConnectFromRandomRoom(Room startRoom, List<Room> column)
    {
        //随机选择房间
        Room targetRoom = column[Random.Range(0, column.Count)];
        //连接房间
        LineRenderer line = Instantiate(linePrefab,transform);
        line.SetPosition(1, startRoom.transform.position);
        line.SetPosition(0, targetRoom.transform.position);
        lineList.Add(line);
        
        return targetRoom;
    }

    private Room ConnectToRandomRoom(Room startRoom, List<Room> column)
    {
        //随机选择房间
        Room targetRoom = column[Random.Range(0, column.Count)];
        //连接房间
        LineRenderer line = Instantiate(linePrefab,transform);
        line.SetPosition(0, startRoom.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        lineList.Add(line);
        
        return targetRoom;
    }

    [ContextMenu("重新生成地图")]
    public void ReGenerateMap()
    {
        foreach (var room in roomList)
        {
            Destroy(room.gameObject);
        }
        foreach (var line in lineList)
        {
            Destroy(line.gameObject);
        }
        
        lineList.Clear();
        roomList.Clear();
        
        CreateMap();
    }
    
    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDic[roomType];
    }
    
    private RoomType GetRandomRoomType(RoomType flags)
    {
        //先进行切分
        string[] options = flags.ToString().Split(',');
        
        string randomOption = options[Random.Range(0, options.Length)];
        
        return (RoomType)Enum.Parse(typeof(RoomType), randomOption);
    }

    private void SaveMap()
    {
        //添加所有房间
        mapLayout.mapRoomDataList = new List<MapRoomData>();
        foreach (var room in roomList)
        {
            MapRoomData mapRoomData = new MapRoomData();
            mapRoomData.posX = room.transform.position.x;
            mapRoomData.posY = room.transform.position.y;
            mapRoomData.column = room.column;
            mapRoomData.row = room.line;
            mapRoomData.roomData = room.roomDataSO;
            mapRoomData.roomState = room.roomState;
            mapLayout.mapRoomDataList.Add(mapRoomData);
        }
        
        //添加所有连线
        mapLayout.linePosList = new List<linePos>();
        foreach (var line in lineList)
        {
            linePos linePos = new linePos();
            linePos.startPos = new SerializeVector3(line.GetPosition(0));
            linePos.endPos = new SerializeVector3(line.GetPosition(1));
            mapLayout.linePosList.Add(linePos);
        }
        
    }
    
    private void LoadMap()
    {
        //读取房间数据生成房间
        foreach (var mapRoomData in mapLayout.mapRoomDataList)
        {
            //实例化房间
            Room room = Instantiate(roomPrefab, new Vector3(mapRoomData.posX, mapRoomData.posY, 0), Quaternion.identity,transform);
            //设置房间数据
            room.SetUpRoom(mapRoomData.column, mapRoomData.row, mapRoomData.roomData);
            //设置房间状态
            room.roomState = mapRoomData.roomState;
            
            roomList.Add(room);
        }
        
        //读取生成连线
        foreach (var linePos in mapLayout.linePosList)
        {
            //实例化连线
            LineRenderer line = Instantiate(linePrefab,transform);
            line.SetPosition(0, linePos.startPos.ToVector3());
            line.SetPosition(1, linePos.endPos.ToVector3());
            
            lineList.Add(line);
        }
    }
}
