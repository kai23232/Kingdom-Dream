using UnityEngine;
using UnityEngine.UIElements;

public class GameplayPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Label energyAmountLabel,drawAmountLabel,discardAmountLabel,turnLabel;
    private Button endTurnButton;
    
    
    [Header("广播")]
    public ObjectEventSO enemyTurnBeginEvent;
    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        
        energyAmountLabel = rootElement.Q<Label>("EnergyAmount");
        drawAmountLabel = rootElement.Q<Label>("DrawAmount");
        discardAmountLabel = rootElement.Q<Label>("DiscardAmount");
        turnLabel = rootElement.Q<Label>("TurnLable");
        endTurnButton = rootElement.Q<Button>("EndTurn");

        drawAmountLabel.text = "0";
        discardAmountLabel.text = "0";
        energyAmountLabel.text = "0";
        turnLabel.text = "玩家回合";
        turnLabel.style.color = Color.green;

        endTurnButton.clicked += OnEndTurnButtonClick;
    }
    public void UpDateEnergyAmount(int amount)
    {
        energyAmountLabel.text = amount.ToString();
    }
    public void UpdateDrawAmount(int amount)
    {
        drawAmountLabel.text = amount.ToString();
    }
    
    public void UpdateDiscardAmount(int amount)
    {
        discardAmountLabel.text = amount.ToString();
    }
    
    private void OnEndTurnButtonClick()
    {
        enemyTurnBeginEvent.RaiseEvent(null,this);
    }

    public void OnEnemyTurnBegin()
    {
        endTurnButton.SetEnabled(false);
        turnLabel.text = "敌人回合";
        turnLabel.style.color = Color.red;
    }
    
    public void OnPlayerTurnBegin()
    {
        endTurnButton.SetEnabled(true);
        turnLabel.text = "玩家回合";
        turnLabel.style.color = Color.green;
    }
}
