using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffct", menuName = "Card/Effect/DamageEffct")]
public class DamageEffct : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (target == null) return;
        switch (targetType)
        {
            case EffectTargetType.Target:
                var damageValue = (int)math.round(value * from.baseStrength);
                target.TakeDamage(damageValue);
                //Debug.Log($"对{target.name}造成了{value}点伤害");
                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;
        }
    }
}
