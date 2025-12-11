using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject arrowPrefab;
    public GameObject currentArrow;
    
    private Card currentCard;
    private bool canMove;
    private bool canExecute;
    
    private CharacterBase targetCharacter;

    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }

    private void OnDisable()
    {
        canMove = false;
        canExecute = false;
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailable)
            return;
        switch (currentCard.CardData.cardType)
        {
            case CardType.Attack:
                currentArrow = Instantiate(arrowPrefab, transform);
                break;
            case CardType.Abilities:
            case CardType.Defense:
                canMove = true;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailable)
            return;
        if (canMove)
        {
            currentCard.isAnimating = true;
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            currentCard.transform.position = worldPos;
            canExecute = worldPos.y > 1f;
        }
        else
        {
            if (eventData.pointerEnter == null) return;

            if (eventData.pointerEnter.CompareTag("Enemy"))
            {
                canExecute = true;
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }

            canExecute = false;
            targetCharacter = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailable)
            return;
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }
        if (canExecute)
        {
            //执行卡牌能力
            currentCard.ExecuteCardEffect(currentCard.player,targetCharacter);
        }
        else
        {
            currentCard.isAnimating = false;
            currentCard.ResetTransform();
        }
    }
    
}
