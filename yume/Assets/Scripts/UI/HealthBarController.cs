using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private CharacterBase currentCharacter;
    [Header("Element")]
    public Transform HealthBarTransform;
    private UIDocument HealthBarDocument;
    private ProgressBar healthBar;

    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
        InitHealthBar();
    }

    private void MoveToWorldPosition(VisualElement element, Vector3 worldPosition,Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, size,Camera.main);
        element.transform.position = rect.position;
    }

    private void InitHealthBar()
    {
        HealthBarDocument = GetComponent<UIDocument>();
        healthBar = HealthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");
        MoveToWorldPosition(healthBar, HealthBarTransform.position, Vector2.zero);
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (currentCharacter.isDead)
        {
            healthBar.style.display = DisplayStyle.None;
            return;
        }

        if (healthBar != null)
        {
            healthBar.title = currentCharacter.CurrentHp + "/" + currentCharacter.MaxHp;
            healthBar.highValue = currentCharacter.MaxHp;
            healthBar.value = currentCharacter.CurrentHp;
            
            healthBar.RemoveFromClassList("highHealth");
            healthBar.RemoveFromClassList("mediumHealth");
            healthBar.RemoveFromClassList("lowHealth");

            var percentage = (float)currentCharacter.CurrentHp / currentCharacter.MaxHp;

            if (percentage < 0.3f)
            {
                healthBar.AddToClassList("lowHealth");
            }
            else if (percentage < 0.6f)
            {
                healthBar.AddToClassList("mediumHealth");
            }
            else
            {
                healthBar.AddToClassList("highHealth");
            }
        }
    }
}
