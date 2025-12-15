using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    protected Animator animator;

    public bool isDead;
    [Header("基础属性")]
    public int maxHp;
    public IntVariable hp;
    public IntVariable defense;
    public int CurrentHp{get => hp.currentValue; set => hp.SetValue(value);}
    public int MaxHp{get => hp.maxValue;}

    [Header("Buff")] 
    public GameObject buff;
    public GameObject debuff;
    
    [Header("力量相关")]
    //力量相关
    public float baseStrength = 1f;
    public float strengthEffct = 0.5f;
    public IntVariable buffRound;

    [Header("广播")] 
    public ObjectEventSO characterDeadEvent;
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHp = MaxHp;
        buffRound.SetValue(0);
    }
    
    public virtual void TakeDamage(int damage)
    {
        var currentDamage = (damage - defense.currentValue) >= 0 ? (damage - defense.currentValue) : 0;
        var currentdefense = (damage - defense.currentValue) >= 0 ? 0 : defense.currentValue - damage;
        if (CurrentHp > currentDamage)
        {
            CurrentHp -= currentDamage;
            //Debug.Log("当前血量：" + CurrentHp);
            animator.SetTrigger("hit");
        }
        else
        {
            CurrentHp = 0;
            //死亡
            isDead = true;
            characterDeadEvent.RaiseEvent(this,this);
            animator.SetBool("isDead",isDead); 
        }
    }
    
    public void Updatedefense(int amount)
    {
        this.defense.currentValue += amount;
    }
    
    public void ResetDefense()
    {
        this.defense.SetValue(0);
    }
    
    public void HpHeal(int amount)
    {
        buff.SetActive(true);
        hp.currentValue += amount;
        if (CurrentHp > MaxHp)
        {
            CurrentHp = MaxHp;
        }
    }
    
    public void SetUpStrength(int round,bool isPositive)
    {
        if (isPositive)
        {
            float newStrength = baseStrength +  strengthEffct;
            baseStrength = Mathf.Min(newStrength,1.5f);
            buff.SetActive(true);
        }
        else
        {
            float newStrength = baseStrength -  strengthEffct;
            baseStrength = Mathf.Max(newStrength,0.5f);
            debuff.SetActive(true);
        }
        
        var currentRound = buffRound.currentValue + round;
        
        if(baseStrength == 1)
            buffRound.SetValue(0);
        else
            buffRound.SetValue(currentRound);
    }
    
    /// <summary>
    /// 回合转换事件函数
    /// </summary>
    public void UpdateStrengthRound()
    {
        buffRound.SetValue(buffRound.currentValue - 1);
        if(buffRound.currentValue <= 0)
        {
            buffRound.SetValue(0);
            baseStrength = 1f;
        }
    }
}
