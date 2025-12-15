using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("面板")]
    public GameObject gameplayPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public GameObject pickCardPanel;

    public void OnLoadRoomEvent(object data)
    {
        Room currentRoom = data as Room;
        switch (currentRoom.roomDataSO.roomType)
        {
            case RoomType.EliteEnemy:
            case RoomType.MinorEnemy:
            case RoomType.Boss:
                gameplayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.RestRoom:
                break;
        }
    }
    
    public void HideAllPanels()
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void OnGameWinEvent()
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }
    
    public void OnGameOverEvent()
    {
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }
    
    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }
}
