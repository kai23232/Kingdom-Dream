using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool PoolTool;
    public List<CardDataSO> cardDataList; //游戏中所有可能出现的卡牌

    private void Awake()
    {
        InitializeCardDataList();
    }

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
}
