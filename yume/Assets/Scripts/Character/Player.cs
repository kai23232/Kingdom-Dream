using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerMana;
    public int maxMana;
    
    public int CurrentMana
    {
        get { return playerMana.currentValue; }
        set { playerMana.SetValue(value); }
    }

    private void OnEnable()
    {
        playerMana.maxValue = maxMana;
        CurrentMana = maxMana;
    }
    
    /// <summary>
    /// 监听事件函数
    /// </summary>
    public void NewTurn()
    {
        CurrentMana = maxMana;
        ResetDefanse();
    }

    public void UpdateMana(int cost)
    {
        CurrentMana -= cost;
        if(CurrentMana < 0)
        {
            CurrentMana = 0;
        }
    }
}
