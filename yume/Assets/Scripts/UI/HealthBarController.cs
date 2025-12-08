using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    [Header("Element")]
    public Transform HealthBarTransform;
    private UIDocument HealthBarDocument;
    private ProgressBar headlthBar;

    private void Awake()
    {
        HealthBarDocument = GetComponent<UIDocument>();
        headlthBar = HealthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");
        MoveToWorldPosition(headlthBar, HealthBarTransform.position, Vector2.zero);
    }

    private void MoveToWorldPosition(VisualElement element, Vector3 worldPosition,Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, size,Camera.main);
        element.transform.position = rect.position;
    }
    
}
