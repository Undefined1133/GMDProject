using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public GameObject floatingTextPrefab;
    public GameObject goldDrop;
    public float minGoldToDrop;
    public float maxGoldToDrop;
    public float minExpToDrop;
    public float maxExpToDrop;
    private PlayerManager playerManager;
    private GameAudioManager gameAudioManager;
    private const float dropChance = 1f / 10f;
    public GameObject gettingHitAnimation;
    private GameObject instantiatedHitAnimation;
    private GameObject instantiatedDeadAnimation;
    public GameObject helmetOfFire;
    public GameObject swordOfEternity;
    public GameObject helmetOfProtection;
    public HealthBar healthBar;
    private int goldPileAmount;
    public TextMeshProUGUI currentMaxHpDisplay;
    private PlayerStats playerStats;
    public float destroyEnemyTime;
    public AudioClip goldDropClip;

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth.GetValue());
        currentMaxHpDisplay.text = currentHealth.GetValue() + "/" + maxHealth.GetValue();
        playerManager = PlayerManager.instance;
        gameAudioManager = GameAudioManager.instance;
        playerStats = playerManager.player.GetComponent<PlayerStats>();
    }

    public override void Die()
    {
        base.Die();
        if (playerManager.quest.isActive)
        {
            playerManager.quest.goal.EnemyKilled();
            playerManager.OnHunterQuestKilled();
            if (playerManager.quest.goal.IsReached())
            {
                playerStats.OnExpGained(playerManager.quest.experienceReward);
                playerManager.AddGold(playerManager.quest.goldReward);
                playerManager.quest.Complete();
            }
        }

        var equipmentToDrop = new List<GameObject>();
        equipmentToDrop.Add(swordOfEternity);
        //Destroying animation here in case if enemy dies
        StopHitAnimation();
        var gold = goldDrop.GetComponent<Gold>();
        gold.SetAmount(minGoldToDrop, maxGoldToDrop);
        DropCoinsOnGround(goldDrop, transform.position, 3f);
        DropExp();
        if (Random.Range(0f, 1f) <= dropChance) DropItemsOnGround(equipmentToDrop, transform.position, 3f);
        //Invoking death to use animation in controller class :D
        Invoke(nameof(DestroyEnemy), destroyEnemyTime);
    }


    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        if (floatingTextPrefab) ShowFloatingText(damage);
        if (gettingHitAnimation != null)
        {
            instantiatedHitAnimation = Instantiate(gettingHitAnimation, transform.position, Quaternion.identity);
            //Invoking in case if enemy doesnt die
            Invoke(nameof(StopHitAnimation), 0.4f);
        }

        base.TakeDamage(damage);
        ShowHpBar();
    }

    private void ShowFloatingText(int damage)
    {
        var instantiatedFloatText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        instantiatedFloatText.GetComponent<TextMeshPro>().text = damage.ToString();
    }

    private void ShowHpBar()
    {
        Debug.Log("Setting health after receiving damageeee");

        healthBar.SetHealth(currentHealth.GetValue());
        currentMaxHpDisplay.text = currentHealth.GetValue() + "/" + maxHealth.GetValue();
    }

    private void StopHitAnimation()
    {
        if (instantiatedHitAnimation != null) Destroy(instantiatedHitAnimation);
    }

    private void DropExp()
    {
        var exp = Random.Range(minExpToDrop, maxExpToDrop);
        var playerStats = playerManager.player.GetComponent<PlayerStats>();
        playerStats.OnExpGained(exp);
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("COLLIDED :DDD");
    }

    private void DropItemsOnGround(List<GameObject> itemsToDrop, Vector3 position, float radius)
    {
        var randomItemIndex = Random.Range(0, itemsToDrop.Count);
        var randomPoint = position + Random.insideUnitSphere * radius;
        // Use Terrain.SampleHeight to get the height of the terrain at the random point
        // Not using terrain high for now
        var enemyHigh = transform.position.y;
        // Instantiate the object at the random point with the y-coordinate set to the terrain height
        randomPoint.y = enemyHigh;
        if (itemsToDrop[randomItemIndex] != null)
            Instantiate(itemsToDrop[randomItemIndex], randomPoint, Quaternion.identity);
        else
            Debug.LogError("NO ITEMS IN LIST :D");
    }

    private void DropCoinsOnGround(GameObject goldDrop, Vector3 position, float radius)
    {
        //Amount of GoldPiles to be a random number from 1 - 10
        goldPileAmount = Random.Range(1, 10);
        // Get a reference to the active terrain in the scene
        //With the new terrain this one stopped working because of terrain high, but will leave for now
        var terrain = Terrain.activeTerrain;
        if (goldDropClip != null) gameAudioManager.PlaySound(goldDropClip);

        for (var i = 0; i < goldPileAmount; i++)
        {
            var randomPoint = position + Random.insideUnitSphere * radius;
            // Use Terrain.SampleHeight to get the height of the terrain at the random point
            // Not using terrain high for now
            var enemyHigh = transform.position.y;
            // Instantiate the object at the random point with the y-coordinate set to the terrain height
            randomPoint.y = enemyHigh;
            Instantiate(goldDrop, randomPoint, Quaternion.identity);
        }
    }
}