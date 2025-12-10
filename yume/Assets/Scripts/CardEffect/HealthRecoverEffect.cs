using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthRecoverEffect",menuName = "Card/Effect/HealthRecoverEffect")]
public class HealthRecoverEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == EffectTargetType.Selt)
        {
            from.HpHeal(value);
        }
        else if (targetType == EffectTargetType.Target)
        {
            target.HpHeal(value);
        }
    }
}
