using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public float offsetSpeed = 0.1f;
    
    private void Update()
    {
        if(lineRenderer != null)
        {
            //获取当前偏移值
            var offset = lineRenderer.material.mainTextureOffset;
            //更新偏移值
            offset.x -= offsetSpeed * Time.deltaTime;
            //设置新的偏移值
            lineRenderer.material.mainTextureOffset = offset;
        }
    }
}
