using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Card/Effect/DefenseEffect")]
public class DefenseEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == EffectTargetType.Self)
        {
            from.Updatedefense(value);
        }
        else if(targetType == EffectTargetType.Target)
        {
            target.Updatedefense(value);
        }
    }
}
