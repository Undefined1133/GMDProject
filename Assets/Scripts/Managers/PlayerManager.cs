using Cinemachine;
using TMPro;
using UnityEngine;

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
    private LogManager logManager;
    public Quest quest;
    public TextMeshProUGUI currentQuestStatusText;
    public TextMeshProUGUI currentLevel;
    public GameObject winPanel;
    public GameObject deathPanel;

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
        logManager = LogManager.instance;
        equipmentManager = EquipmentManager.instance;
        gameAudioManager = GameAudioManager.instance;
        playerStats = player.GetComponent<PlayerStats>();
        playerController = player.GetComponent<PlayerController>();
        playerStats.onLevelUp += OnLevelUp;
        gold = 0f;
        currentLevel.text = playerStats.level.ToString();
    }

    #endregion

    public void KillPlayer()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Respawn()
    {
        if (deathPanel != null && deathPanel.activeSelf)
        {
            deathPanel.SetActive(false);
        }
        playerStats.isDead = false;
        player.transform.position = new Vector3(825, 146, 4412);

    }

    public void ContinuePlaying()
    {
        if (winPanel != null && winPanel.activeSelf)
        {
            winPanel.SetActive(false);
        }
    }

    public void OnHunterQuestKilled()
    {
        currentQuestStatusText.text = quest.goal.currentAmount + "/" + quest.goal.requiredAmount;
        if (quest.goal.goalType == GoalType.Boss)
        {
            winPanel.SetActive(true);
        }
    }

    public void OnBossQuestKilled()
    {
        currentQuestStatusText.text = quest.goal.currentAmount + "/" + quest.goal.requiredAmount;
        if (quest.goal.goalType == GoalType.Boss)
        {
            winPanel.SetActive(true);
        }
    }

    public void AddGold(float amount)
    {
        gold += amount;
        logManager.GoldGainedLog("+ " + (int) amount + " Gold");
        SetGold(gold);
    }

    public void OnLevelUp()
    {
        currentLevel.text = playerStats.level.ToString();
    }

    public void RemoveGold(float amount)
    {
        gold -= amount;
        logManager.ErrorLog("- " + (int) amount + " Gold");
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