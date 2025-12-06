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

    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        switch (currentCard.CardData.cardType)
        {
            case CardTypte.Attack:
                currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                break;
            case CardTypte.Abilities:
            case CardTypte.Defense:
                canMove = true;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canMove)
        {
            currentCard.isAnimating = true;
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            currentCard.transform.position = worldPos;
            canExecute = worldPos.y > 1f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }
        if (canExecute)
        {
            //TODO: 执行卡牌能力
        }
        else
        {
            currentCard.isAnimating = false;
            currentCard.ResetTransform();
        }
    }
}
