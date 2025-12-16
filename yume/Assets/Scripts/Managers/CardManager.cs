using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList; //游戏中所有可能出现的卡牌

    [Header("卡牌库")]
    public CardLibrarySO newGameCardLibrary;
    public CardLibrarySO currentCardLibrary;

    private int previousIndex = 0;
    
    private void Awake()
    {
        InitializeCardDataList();

        foreach (var entry in newGameCardLibrary.cardLibraryList)
        {
            currentCardLibrary.cardLibraryList.Add(entry);
        }
    }

    #region 获得项目中的卡牌

    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaed;
    }

    private void OnCardDataLoaed(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("加载卡牌数据失败");
        }
    }

    #endregion
    
    /// <summary>
    /// 抽卡时调用的函数获得卡牌GameObject
    /// </summary>
    /// <returns></returns>
    public GameObject GetCardObject()
    {
        var card = poolTool.GetObjectFromPool();
        card.transform.localScale = Vector3.zero;
        return card;
    }
    
    public void DiscardCard(GameObject card)
    {
        poolTool.ReleaseObjectToPool(card);
    }

    private void OnDisable()
    {
        currentCardLibrary.cardLibraryList.Clear();
    }
    
    public CardDataSO GetNewCardData()
    {
        int randomIndex = 0;
        do
        {
            randomIndex = Random.Range(0, cardDataList.Count);
        } while (previousIndex == randomIndex);
        previousIndex = randomIndex;
        return cardDataList[randomIndex];
    }
    
    /// <summary>
    /// 解锁新的卡牌
    /// </summary>
    /// <param name="newCardData"></param>
    public void UnLockCard(CardDataSO newCardData)
    {
        
        int targetIndex = currentCardLibrary.cardLibraryList.FindIndex(x => x.cardData == newCardData);

        if (targetIndex == -1)
        {
            var newCard = new CardLibraryEntry
            {
                cardData = newCardData,
                amount = 1
            };
            currentCardLibrary.cardLibraryList.Add(newCard);
        }
        else
        {
            CardLibraryEntry target = currentCardLibrary.cardLibraryList[targetIndex];
            target.amount++;
            currentCardLibrary.cardLibraryList[targetIndex] = target;
        }
    }
}
