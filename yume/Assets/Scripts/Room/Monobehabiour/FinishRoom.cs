using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRoom : MonoBehaviour
{
    public ObjectEventSO loadMapEventSO; 
    private void OnMouseDown()
    {
        //返回地图
        loadMapEventSO.RaiseEvent(null,this);
    }
}
