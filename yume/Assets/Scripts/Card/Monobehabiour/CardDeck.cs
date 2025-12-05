using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;


public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    private List<CardDataSO> drawDeck = new List<CardDataSO>(); //抽牌库
    private List<CardDataSO> discardDeck = new List<CardDataSO>(); //弃牌库
    private List<Card> handCardObjectList = new List<Card>(); //当前手牌（每回合）
    
    public Vector3 DeckPosition;

    //TODO:测试用
    private void Start()
    {
        InitializeDeck();
        
        DrawCard(3);
    }

    public void InitializeDeck()
    {
        drawDeck.Clear();

        foreach (var cardEntry in cardManager.currentCardLibrary.cardLibraryList)
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

            var card = cardManager.GetCardObject().GetComponent<Card>();
            //初始化
            card.Init(cardData);
            //设置卡牌位置
            card.transform.position = DeckPosition;
            
            handCardObjectList.Add(card);
            
            float delay = i * 0.2f;
            SetCardLayout(delay);
        }
    }

    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard = handCardObjectList[i];
            
            CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCardObjectList.Count);
            
            //currentCard.transform.SetPositionAndRotation(cardTransform.pos.ToVector3(), cardTransform.rotation);
            
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).OnComplete(() =>
            {
                currentCard.transform.DOMove(cardTransform.pos, 0.5f);
                currentCard.transform.DORotate(cardTransform.rotation.eulerAngles, 0.5f);
            });
            
            //设置卡牌排序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
        }
    }
}
