using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    private Button backToStartButton;
    
    [Header("广播")]
    public ObjectEventSO loadMenuEvent;

    private void OnEnable()
    {
        backToStartButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToStartButton");
        backToStartButton.clicked += OnBackToStartButtonClicked;
    }

    private void OnBackToStartButtonClicked()
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
