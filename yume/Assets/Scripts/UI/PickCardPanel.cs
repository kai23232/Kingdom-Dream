using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PickCardPanel : MonoBehaviour
{
    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;
    private VisualElement cardContainer;
    private CardDataSO currentCardData;
    private Button confirmButton;

    public CardManager cardManager;
    
    private List<Button> cardButtons = new List<Button>();
    
    [Header("广播")]
    public ObjectEventSO finishPickCardEvent;
    

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("ConfirmButton");
        confirmButton.clicked += OnConfirmButtonClicked;
        
        for (int i = 0; i < 3; i++)
        {
            var card = cardTemplate.Instantiate();
            var data = cardManager.GetNewCardData();
            InitCard(card,data);
            
            var cardButton = card.Q<Button>("Card");
            cardContainer.Add(card);
            cardButtons.Add(cardButton);
            cardButton.clicked += () => OnCardClicked(cardButton,data);
            
            //防止玩家没有选择卡牌就进行点击
            currentCardData = data;
        }
    }
        
    private void OnDisable()
    {
        cardButtons.Clear();
        cardContainer.Clear();
    }

    private void OnConfirmButtonClicked()
    {
        cardManager.UnLockCard(currentCardData);
        finishPickCardEvent.RaiseEvent(this,this);
    }
        
    private void OnCardClicked(Button cardButton, CardDataSO data)
    {
        //Debug.Log("点击了卡牌：" + data.name);
        for (int i = 0; i < cardButtons.Count; i++)
        {
            if(cardButton == cardButtons[i]) 
                cardButtons[i].SetEnabled(false);
            else
            {
                cardButtons[i].SetEnabled(true);
            }
        }
        currentCardData = data;
    }

    public void InitCard(VisualElement card,CardDataSO cardData)
    {
        var cardSpriteElement = card.Q<VisualElement>("CardSprite");
        var cardCost = card.Q<Label>("EnergyCost");
        var cardDescription = card.Q<Label>("CardDescription");
        var cardType = card.Q<Label>("CardType");
        var cardName = card.Q<Label>("CardName");

        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardIcon);
        cardCost.text = cardData.cost.ToString();
        cardDescription.text = cardData.description;
        cardType.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Abilities => "能力",
            _ => "未知"
        };
        cardName.text = cardData.name;
        
    }
    
}
