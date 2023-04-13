using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private int baseValue;
    private List<int> modifiers = new();

    public int GetValue()
    {
        var finalValue = baseValue;
        modifiers.ForEach(value => finalValue += value);
        return finalValue;
    }

    public void SetValue(int value)
    {
        baseValue = value;
    }

    public void AddModifier(int modifier)
    {
        if (modifier != 0) modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0) modifiers.Remove(modifier);
    }

    public void IncreaseHealth(int level)
    {
        SetValue(baseValue += Mathf.RoundToInt(baseValue * 0.01f * ((100 - level) * 0.1f)));
    }
}