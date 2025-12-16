using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    public AssetReference map;
    public AssetReference menu;
    
    private Vector2Int currentRoomVector;
    
    [Header("广播")]
    public ObjectEventSO afterLoadRoomEvent;

    public ObjectEventSO updateRoomEvent;

    private void Start()
    {
        currentRoomVector = Vector2Int.one * -1;
        LoadMenu();
    }

    /// <summary>
    /// 在房间加载事件中监听
    /// </summary>
    /// <param name="data"></param>
    public void OnLoadRoomEvent(object data)
    {
        Room currentRoom = data as Room;
        if (data is Room)
        {
            currentRoomVector = new Vector2Int(currentRoom.column, currentRoom.line);

            currentScene = currentRoom.roomDataSO.sceneToLoad;
        }
        
        StartCoroutine(LoadAndUnload(currentRoom));
    }
    
    //卸载场景后加载房间
    private IEnumerator LoadAndUnload(Room room)
    {
        //卸载当前场景
        yield return StartCoroutine(UnloadSceneAsync());
        //加载房间
        yield return StartCoroutine(LoadSceneAsync());
        
        //广播加载完成事件
        afterLoadRoomEvent.RaiseEvent(room, this);
    }
    //异步加载场景
    private IEnumerator LoadSceneAsync()
    {
        //加载当前场景
        var operation = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        yield return operation;
        SceneManager.SetActiveScene(operation.Result.Scene);
        
    }
    
    private IEnumerator UnloadSceneAsync()
    {
        //卸载当前场景
        var operation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        yield return operation;
    }

    public void LoadMap()
    {
        StartCoroutine(LoadMapAsync());
    }
    private IEnumerator LoadMapAsync()
    {
        yield return StartCoroutine(UnloadSceneAsync());
        if(currentRoomVector != Vector2Int.one * -1 && currentScene != menu)
            updateRoomEvent.RaiseEvent(currentRoomVector, this);
        currentScene = map;
        StartCoroutine(LoadSceneAsync());
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadManuAsync());
    }
    private IEnumerator LoadManuAsync()
    {
        if(currentScene != null)
            yield return StartCoroutine(UnloadSceneAsync());
        currentScene = menu;
        StartCoroutine(LoadSceneAsync());
    }
}
