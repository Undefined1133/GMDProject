using TMPro;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    // Start is called before the first frame update
    public HealthBar healthBar;
    public ManaBar manaBar;
    public ExpBar expBar;
    public TextMeshProUGUI currentMaxHealth;
    public TextMeshProUGUI currenMaxMana;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI minMaxDamage;
    public TextMeshProUGUI currentMaxExperience;
    public TextMeshProUGUI currentArmor;
    public float expTillNextLevel;
    public float exp;
    public int level;
    public float additionMultiplier = 300;
    public float powerMultiplier = 2;
    public float divisionMultiplier = 7;
    public delegate void OnLevelUp();
    public OnLevelUp onLevelUp;
    public delegate void OnHeal();
    public OnHeal onHeal;
    public delegate void TakenDamageEventHandler();
    public event TakenDamageEventHandler takenDamage;
    public delegate void DiedEventHandler();
    public event DiedEventHandler died;
    

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth.GetValue());
        manaBar.SetMaxMana(maxMana.GetValue());
        expTillNextLevel = CalculateRequiredExp();
        OnExpChanged();
        OnDamageChanged();
        OnExpChanged();
        OnHpChanged();
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) TakeDamage(damage.GetValue());
    }

    public void OnManaUsed()
    {
        currenMaxMana.text = currentMana.GetValue() + "/" + maxMana.GetValue();
    }

    public void OnExpChanged()
    {
        currentMaxExperience.text = Mathf.RoundToInt(exp) + "/" + expTillNextLevel;
        expBar.SetMaxExp(expTillNextLevel);
    }
    public void OnDamageChanged()
    {
        minMaxDamage.text = minDamage.GetValue() + "-" + maxDamage.GetValue();
    }

    public void OnSpeedChanged()
    {
        speed.text = movementSpeed.GetValue().ToString();
    }
    public void OnArmorChanged()
    {
        currentArmor.text = armor.GetValue().ToString();
    }

    public void OnHpChanged()
    {
        currentMaxHealth.text = currentHealth.GetValue() + "/" + maxHealth.GetValue();
        healthBar.SetMaxHealth(maxHealth.GetValue());
        healthBar.SetHealth(currentHealth.GetValue());
    }

    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
            minDamage.AddModifier(newItem.minDamageModifier);
            maxDamage.AddModifier(newItem.maxDamageModifier);
            OnDamageChanged(); 
            OnArmorChanged();
            Debug.Log("Min max damage = " + minDamage.GetValue() + " " + maxDamage.GetValue());
        }
        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
            minDamage.RemoveModifier(oldItem.minDamageModifier);
            maxDamage.RemoveModifier(oldItem.maxDamageModifier);
            OnDamageChanged(); 
            OnArmorChanged();
        }
    }

    public void OnExpGained(float gainedExp)
    {
        exp += gainedExp;
        if (exp > expTillNextLevel)
        {
            LevelUp();
        }
        expBar.SetExp(exp);
        OnExpChanged();
        Debug.Log("EXPPPPP = " + exp);
    }

    public void Heal(int healAmount)
    {
        if (currentHealth.GetValue() + healAmount >= maxHealth.GetValue())
            currentHealth.SetValue(maxHealth.GetValue());
        else
            currentHealth.SetValue(currentHealth.GetValue() + healAmount);
        OnHpChanged();
        onHeal?.Invoke();
    }

    public void LevelUp()
    {
        level++;
        expBar.SetExp(0);
        exp = Mathf.RoundToInt(exp - expTillNextLevel);
        expBar.SetExp(exp);
        maxHealth.IncreaseHealth(level);
        currentHealth.SetValue(maxHealth.GetValue());
        //Updating HP Ui
        OnHpChanged();
        expTillNextLevel = CalculateRequiredExp();
        //Updating exp :D
        OnExpChanged();
        //Invoking level up to initialize animation on the player controller
        onLevelUp?.Invoke();
    }

    private int CalculateRequiredExp()
    {
        int requiredExp = 0;
        for (int levelIteration = 1; levelIteration <= level; levelIteration++)
        {
            requiredExp += (int)Mathf.Floor(levelIteration +
                                       additionMultiplier * Mathf.Pow(powerMultiplier,
                                           levelIteration / divisionMultiplier));
        }

        return requiredExp / 4;
    }

    public override void Die()
    {
        base.Die();
        //Kill the player somehow
        died?.Invoke();

        PlayerManager.instance.KillPlayer();
    }

    public override void TakeDamage(int damage)
    {
        //Invoking take damage to run get hit animation :OO
        takenDamage?.Invoke();
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        currentHealth.SetValue(currentHealth.GetValue() - damage);
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth.GetValue() <= 0) Die();
        healthBar.SetHealth(currentHealth.GetValue());
        OnHpChanged();
    }
}