using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList; //游戏中所有可能出现的卡牌

    [Header("卡牌库")]
    public CardLibrarySO newGameCardLibrary;
    public CardLibrarySO currentCardLibrary;
    
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

    public GameObject GetCardObject()
    {
        return poolTool.GetObjectFromPool();
    }
    
    public void DiscardCard(GameObject card)
    {
        poolTool.ReleaseObjectToPool(card);
    }   
}
