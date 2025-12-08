using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
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
        if (CurrentHp > damage)
        {
            CurrentHp -= damage;
            Debug.Log("当前血量：" + CurrentHp);
        }
        else
        {
            CurrentHp = 0;
            //死亡
            isDead = true;
        }
    }
}
