using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
    public IntVariable defanse;
    public int CurrentHp{get => hp.currentValue; set => hp.SetValue(value);}
    public int MaxHp{get => hp.maxValue;}

    protected Animator animator;

    public bool isDead;
    
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHp = MaxHp;
    }
    
    public virtual void TakeDamage(int damage)
    {
        var currentDamage = (damage - defanse.currentValue) >= 0 ? (damage - defanse.currentValue) : 0;
        var currentDefanse = (damage - defanse.currentValue) >= 0 ? 0 : defanse.currentValue - damage;
        if (CurrentHp > currentDamage)
        {
            CurrentHp -= currentDamage;
            Debug.Log("当前血量：" + CurrentHp);
        }
        else
        {
            CurrentHp = 0;
            //死亡
            isDead = true;
        }
    }
    
    public void UpdateDefanse(int amount)
    {
        this.defanse.currentValue += amount;
    }
    
    public void ResetDefanse()
    {
        this.defanse.SetValue(0);
    }
}
