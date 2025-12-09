using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Rendering;


public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    private List<CardDataSO> drawDeck = new List<CardDataSO>(); //抽牌库
    private List<CardDataSO> discardDeck = new List<CardDataSO>(); //弃牌库
    private List<Card> handCardObjectList = new List<Card>(); //当前手牌（每回合）
    
    public Player player;
    
    public Vector3 DeckPosition;
    
    [Header("广播")]
    public IntEventSO drawAmountEvent;
    public IntEventSO discardAmountEvent;
    
    
    private void Start()
    {
        InitializeDeck();
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
        
        ShuffleDeck();
    }
    
    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }
    
    public void NewTurnDrawCards(object data)
    {
        player.NewTurn();
        DrawCard(3);
    }
    
    private void DrawCard(int amount)
    {
        for(int i = 0;i < amount;i++)
        {
            if (drawDeck.Count == 0)
            {
                foreach (var cardDataSo in discardDeck)
                {
                    drawDeck.Add(cardDataSo);
                }
                ShuffleDeck();
            }

            CardDataSO cardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            //更新UI显示数量
            drawAmountEvent.RaiseEvent(drawDeck.Count,this);
            
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
    
    /// <summary>
    /// 设置卡牌布局
    /// </summary>
    /// <param name="delay"></param>
    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard = handCardObjectList[i];
            
            CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCardObjectList.Count);
            
            //currentCard.transform.SetPositionAndRotation(cardTransform.pos.ToVector3(), cardTransform.rotation);
            
            //判断卡牌能量
            currentCard.UpdateState();
            
            currentCard.isAnimating = true;
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).OnComplete(() =>
            {
                currentCard.transform.DOMove(cardTransform.pos, 0.5f);
                currentCard.transform.DORotate(cardTransform.rotation.eulerAngles, 0.5f);
            });
            currentCard.isAnimating = false;
            
            //设置卡牌排序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            //更新原始数据
            currentCard.UpdatePositionAndRotation(cardTransform.pos, cardTransform.rotation);
        }
    }
    
    
    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleDeck()
    {
        discardDeck.Clear();
        
        drawAmountEvent.RaiseEvent(drawDeck.Count,this);
        discardAmountEvent.RaiseEvent(discardDeck.Count,this);
        
        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }
    
    /// <summary>
    /// 弃牌
    /// </summary>
    /// <param name="card"></param>
    public void DiscardCard(object data)
    {
        Card card = data as Card;
        discardDeck.Add(card.CardData);
        handCardObjectList.Remove(card);
        cardManager.DiscardCard(card.gameObject);
        
        //更新UI显示数量
        discardAmountEvent.RaiseEvent(discardDeck.Count,this);
        
        SetCardLayout(0f);
    }

    public void OnPlayerTurnEnd()
    {
        for(int i = handCardObjectList.Count - 1;i >= 0;i--)
        {
            DiscardCard(handCardObjectList[i]);
        }
    }
}
