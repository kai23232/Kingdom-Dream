using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public CardManager CardManager;
    private List<CardDataSO> drawDeck = new List<CardDataSO>(); //抽牌库
    private List<CardDataSO> discardDeck = new List<CardDataSO>(); //弃牌库
    private List<Card> handCardObjectList = new List<Card>(); //当前手牌（每回合）

    //TODO:测试用
    private void Start()
    {
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        drawDeck.Clear();

        foreach (var cardEntry in CardManager.currentCardLibrary.cardLibraryList)
        {
            for(int i = 0;i < cardEntry.amount;i++)
            {
                drawDeck.Add(cardEntry.cardData);
            }
        }
        
        //TODO: 洗牌/更新抽牌库和弃牌库的数字
    }
    
    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }
    
    private void DrawCard(int amount)
    {
        for(int i = 0;i < amount;i++)
        {
            if (drawDeck.Count == 0)
            {
                
            }

            CardDataSO cardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            var card = CardManager.GetCardObject().GetComponent<Card>();
            //初始化
            card.Init(cardData);
            handCardObjectList.Add(card);
        }
    }
}
