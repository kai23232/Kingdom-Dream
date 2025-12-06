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
        
        ShuffleDeck();
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
                foreach (var cardDataSo in discardDeck)
                {
                    drawDeck.Add(cardDataSo);
                }
                ShuffleDeck();
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
        //TODO:更新UI显示数量

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
    public void DiscardCard(Card card)
    {
        discardDeck.Add(card.CardData);
        handCardObjectList.Remove(card);
        cardManager.DiscardCard(card.gameObject);
        
        SetCardLayout(0f);
    }
}
