using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card/CardData")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite cardIcon;
    public int cost;
    public CardTypte cardType;
    [TextArea]
    public string description;
    
    //TODO: 增加卡片效果
}
