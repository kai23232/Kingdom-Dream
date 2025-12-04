using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("组件")]
    public SpriteRenderer cardSprite;
    public TextMeshPro costText,descriptionText,typeText,nameText;
    
    public CardDataSO CardData;

    private void Start()
    {
        Init(CardData);
    }

    public void Init(CardDataSO data)
    {
        CardData = data;
        cardSprite.sprite = CardData.cardIcon;
        costText.text = CardData.cost.ToString();
        nameText.text = CardData.name;
        descriptionText.text = CardData.description;
        typeText.text = CardData.cardType switch
        {
            CardTypte.Attack => "攻击",
            CardTypte.Defense => "防御",
            CardTypte.Abilities => "能力",
            _ => "未知"
        };
    }
}
