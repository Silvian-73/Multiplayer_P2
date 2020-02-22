using UnityEngine;
using System;
using System.Collections.Generic;
[System.Serializable]
public class Stat
{
    public event Action<int> OnStatChanged;

    private List<int> _modifiers = new List<int>();

    [SerializeField] int _baseValue;
    public int BaseValue
    {
        get 
        { 
            return _baseValue; 
        }
        set
        {
            _baseValue = value;
            OnStatChanged?.Invoke(GetValue());
        }
    }
    public int GetValue()
    {
        int finalValue = _baseValue;
        _modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }
    public void AddModifier(int modifier)
    {
        if (modifier != 0)
        {
            _modifiers.Add(modifier);
            OnStatChanged?.Invoke(GetValue());
        }
    }
    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
        {
            _modifiers.Remove(modifier);
            OnStatChanged?.Invoke(GetValue());
        }
    }

}