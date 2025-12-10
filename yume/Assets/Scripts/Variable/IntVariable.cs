using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "IntVariable", menuName = "Variable/IntVariable")]
public class IntVariable : ScriptableObject
{
    public int maxValue;
    public int currentValue;
    public IntEventSO onValueChanged;

    [TextArea]
    [SerializeField] private string description;
    
    public void SetValue(int value)
    {
        currentValue = value;
        onValueChanged?.RaiseEvent(value,this);
    }
}
