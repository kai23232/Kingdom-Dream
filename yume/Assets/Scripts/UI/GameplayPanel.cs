using UnityEngine;
using UnityEngine.UIElements;

public class GameplayPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Label energyAmountLabel,drawAmountLabel,discardAmountLabel,turnLabel;
    private Button endTurnButton;

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
    }
    
    public void UpdateDrawAmount(int amount)
    {
        drawAmountLabel.text = amount.ToString();
    }
    
    public void UpdateDiscardAmount(int amount)
    {
        discardAmountLabel.text = amount.ToString();
    }
}
