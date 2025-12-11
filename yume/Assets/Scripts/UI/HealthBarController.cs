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
    
    private VisualElement defenseElement;
    private Label defenseAmountLabel;

    private VisualElement buffElement;
    private Label buffRoundLabel;
    
    [Header("Buff Sprite")]
    public Sprite buffSprite;
    public Sprite debuffSprite;

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
        
        defenseElement = healthBar.Q<VisualElement>("Defense");
        defenseAmountLabel = defenseElement.Q<Label>("DefenseAmount");
        
        buffElement = healthBar.Q<VisualElement>("Buff");
        buffRoundLabel = buffElement.Q<Label>("BuffRound");
        
        buffElement.style.display = DisplayStyle.None;
        defenseElement.style.display = DisplayStyle.None;
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
            
            //更新防御属性
            defenseElement.style.display = currentCharacter.defense.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            defenseAmountLabel.text = currentCharacter.defense.currentValue.ToString();
            
            //更新Buff属性
            buffElement.style.display = currentCharacter.buffRound.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            buffRoundLabel.text = currentCharacter.buffRound.currentValue.ToString();
            //更新Buff图标
            buffElement.style.backgroundImage = currentCharacter.baseStrength > 1f ? new StyleBackground(buffSprite) : new StyleBackground(debuffSprite);
        }
    }
}
