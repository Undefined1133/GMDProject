using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
[SerializeField]
private int baseValue;
private List<int> modifiers = new List<int>();
public int GetValue()
{
	int finalValue = baseValue;
	modifiers.ForEach(value => finalValue += value);
	return finalValue;
}
public void SetValue(int value)
{
	baseValue = value;
}

public void AddModifier(int modifier)
{
	if(modifier !=0)
	{
		modifiers.Add(modifier);
	}
}
public void RemoveModifier(int modifier)
{
	if(modifier != 0)
	{
		modifiers.Remove(modifier);
	}
}
}
