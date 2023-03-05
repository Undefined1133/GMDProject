using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{

#region Singleton

public static PlayerManager instance;
public TextMeshProUGUI goldText;
public float gold;

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
SetGold(gold);
}

public void SavePlayer()
{
	SaveSystem.SavePlayer(this);
}

public void SetGold(float goldAmount)
{
	goldText.text = goldAmount.ToString("F2");

}

public void LoadPlayer()
{
	PlayerData data = SaveSystem.LoadPlayer();
	gold = data.gold;
	SetGold(data.gold);
	Vector3 position;
	position.x = data.position[0];
	position.y = data.position[1];
	position.z = data.position[2];
	player.transform.position = position;
}
}
