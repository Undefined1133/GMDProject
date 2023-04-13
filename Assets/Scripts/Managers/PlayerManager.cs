using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;
    public TextMeshProUGUI goldText;
    public float gold;
    public HealthBar healthBar;
    public ManaBar manaBar;
    public GameObject player;
    public CinemachineFreeLook camera;
    public PlayerStats playerStats;
    private PlayerController playerController;
    private EquipmentManager equipmentManager;
    private GameAudioManager gameAudioManager;
    public Quest quest;
    public TextMeshProUGUI currentQuestStatusText;




    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of PlayerManager found!");
            return;
        }
        instance = this;
        
    }

    private void Start()
    {
        equipmentManager = EquipmentManager.instance;
        gameAudioManager = GameAudioManager.instance;
        playerStats = player.GetComponent<PlayerStats>();
        playerController = player.GetComponent<PlayerController>();
        gold = 0f;
    }

    #endregion

    public void KillPlayer()
    {
        //KIll player
    }
    

    public void OnHunterQuestKilled()
    {
        currentQuestStatusText.text = quest.goal.currentAmount + "/" + quest.goal.requiredAmount;
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
        var data = SaveSystem.LoadPlayer();
        var playerStats = player.GetComponent<PlayerStats>();
        playerStats.currentHealth.SetValue(data.health);
        playerStats.currentMana.SetValue(data.mana);
        playerStats.exp = data.exp;
        playerStats.expBar.SetExp(playerStats.exp);
        healthBar.SetHealth(data.health);
        manaBar.SetMana(data.mana);
        gold = data.gold;
        SetGold(data.gold);
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        player.transform.position = position;
    }
    
}