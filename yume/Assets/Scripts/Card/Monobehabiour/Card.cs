using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [Header("组件")]
    public SpriteRenderer cardSprite;
    public TextMeshPro costText,descriptionText,typeText,nameText;
    
    public CardDataSO CardData;

    [Header("原始数据")] 
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;
    
    public bool isAnimating = false;
    public bool isAvailable;

    public Player player;
    
    [Header("广播")]
    public ObjectEventSO discardCardEvent;

    public IntEventSO costEvent;
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
            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Abilities => "能力",
            _ => "未知"
        };
        
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void UpdatePositionAndRotation(Vector3 position,Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isAnimating) return;
        transform.position = originalPosition + Vector3.up;
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isAnimating) return;
        ResetTransform();
    }
    
    public void ResetTransform()
    {
        transform.SetPositionAndRotation(originalPosition,originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }
    
    public void ExecuteCardEffect(CharacterBase from,CharacterBase target)
    {
        //减少对应能量，通知回收卡牌
        costEvent.RaiseEvent(CardData.cost,this);
        discardCardEvent.RaiseEvent(this,this);
        foreach (var effect in CardData.effects)
        {
            effect.Execute(from,target);
        }
    }

    public void UpdateState()
    {
        isAvailable = player.CurrentMana >= CardData.cost;
        costText.color = isAvailable ? Color.green : Color.red;
    }
}
