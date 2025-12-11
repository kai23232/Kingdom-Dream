using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card/CardData")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite cardIcon;
    public int cost;
    public CardType cardType;
    [TextArea]
    public string description;
    
    //卡片效果
    public List<Effect> effects;
}
