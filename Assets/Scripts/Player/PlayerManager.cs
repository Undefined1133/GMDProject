using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{

#region Singleton

public static PlayerManager instance;
public TextMeshProUGUI goldText;
private float gold;

void Awake()
{
	gold = 0f;
	instance = this;
}

#endregion

public GameObject player;

public void KillPlayer()
{
	Debug.Log("OH NOOO DE PLAYER IZ KILEED");
	//KIll player
}

public void AddGold(float amount)
{

gold += amount;
goldText.text = gold.ToString("F2");
}

}
