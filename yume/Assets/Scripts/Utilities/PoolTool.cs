using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;
    private ObjectPool<GameObject> pool;

    private void Start()
    {
        //初始化对象池
        pool  = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objPrefab,transform),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );
        
        PreFillPool(10);
    }
    
    private void PreFillPool(int count)
    {
        //预填充对象池
        var preFillArray = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = pool.Get();
        }
        
        //将预填充数组中的对象添加到对象池
        foreach (var item in preFillArray)
        {
            pool.Release(item);
        }
    }
    
    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }
    
    public void ReleaseObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
