using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffct", menuName = "Card/Effect/DrawCardEffct")]
public class DrawCardEffct : Effect
{
    public IntEventSO drawCardEvent;
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        drawCardEvent?.RaiseEvent(value,this);
    }
}
