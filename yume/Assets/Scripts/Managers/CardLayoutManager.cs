using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    [Header("横向布局参数")]
    public float maxWidth = 7f;
    public float cardSpacing = 2f;
    [Header("弧形布局参数")] 
    public float maxAngle = 90;
    public float angleBetweenCards = 7f;
    public float radius = 17f;
    
    public Vector3 centerPoint;

    [SerializeField]private List<Vector3> cardPositions = new List<Vector3>();
    [SerializeField]private List<Quaternion> cardRotations = new List<Quaternion>();

    private void Awake()
    {
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }

    public CardTransform GetCardTransform(int index, int totalCards)
    {
        CalculatePosition(totalCards, isHorizontal);
        
        return new CardTransform(cardPositions[index], cardRotations[index]);
    }

    private void CalculatePosition(int numberOfCards, bool horizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();
        if (horizontal)
        {
            float currentWidth = cardSpacing * (numberOfCards-1);
            float totalWidth = Mathf.Min(maxWidth,currentWidth);
            
            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards-1) : 0;

            for (int i = 0; i < numberOfCards; i++)
            {
                float xPos = 0 - (totalWidth / 2) + (i * currentSpacing);

                var pos = new Vector3(xPos, centerPoint.y, 0f);
                var rotation = Quaternion.identity;
                
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
        else
        {
            float cardAngle = (numberOfCards - 1) * angleBetweenCards;
            float totalAngle = Mathf.Min(maxAngle, cardAngle) / 2;
            
            float currentAngleBetweenCards = totalAngle > 0 ? totalAngle * 2 / (numberOfCards - 1) : 0; 
            
            for (int i = 0; i < numberOfCards; i++)
            {
                var pos = FanCardPosition(totalAngle - i * currentAngleBetweenCards);
                
                var rotation  = Quaternion.Euler(0, 0, totalAngle - i * currentAngleBetweenCards);
                
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
            centerPoint.y + Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
            0
        );
    }
    
}
