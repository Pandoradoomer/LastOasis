using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Linq;

public enum Stat
{
    Health,
    Speed,
    Defence,
    Dexterity,
    Damage,
    Coin_Gain,
    Enemy_Dmg,
    Enemy_HP,
    Healing,
    Death_Save,
    Blindness,
    UI_Blindness,
    Current_Health,
    Coin_Loss
}

public enum MaxStat


{
    MaxHealth,
    MaxSpeed,
    MaxDefence,
    MaxDexterity,
    MaxDamage
}

public enum StatModifierType
{
    SET,
    PERCENTAGE,
    NUMERICAL
}

public class StatModifier
{
    public Stat stat;
    public StatModifierType statModifierType;
    public float modifierValue;
    public object modifier;
    public Type modifierType;

    public StatModifier(Stat stat, StatModifierType type, float value)
    {
        this.stat = stat;
        this.statModifierType = type;
        modifierValue = value;
    }
}

public class PlayerStats : MonoBehaviour
{
    //Values that the player is currently holding

    private bool hasBeenInit = false;
    public static PlayerStats Instance { get; private set; }

    [SerializeField]
    private StatValues baseValuesObject;

    public Dictionary<Stat, float> baseStatValues;
    public Dictionary<Stat, float> cachedCalculatedValues;
    public List<StatModifier> statModifiers;

    bool isDead = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;

        }
        statModifiers = new List<StatModifier>();
        if (PlayerPrefs.HasKey("isSet"))
            hasBeenInit = Convert.ToBoolean(PlayerPrefs.GetString("isSet"));
        else
        {
            hasBeenInit = false;
            PlayerPrefs.SetString("isSet", false.ToString());
        }
        if(!hasBeenInit)
        {
            hasBeenInit = true;
            baseStatValues = new Dictionary<Stat, float>();
            cachedCalculatedValues = new Dictionary<Stat, float>();
            foreach(StatValue sv in baseValuesObject.statValues)
            {
                baseStatValues.Add(sv.stat, sv.value);
                cachedCalculatedValues.Add(sv.stat, sv.value);
            }

        }
        else
        {
            LoadValues();
        }
        EventManager.StartListening(Event.EnemyHitPlayer, OnEnemyHit);
        EventManager.StartListening(Event.PlayerDeath, OnPlayerDeath);
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {

    }

    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetStatValues();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            cachedCalculatedValues[Stat.Current_Health] = 0;
        }
        if (IsPlayerDead())
        {
            if (cachedCalculatedValues[Stat.Death_Save] > 0)
            {
                EventManager.TriggerEvent(Event.DeathSave, null);
                cachedCalculatedValues[Stat.Death_Save] = 0;
                cachedCalculatedValues[Stat.Current_Health] = (int)(30.0f * cachedCalculatedValues[Stat.Health] / 100.0f);
                return;

            }
            cachedCalculatedValues[Stat.Current_Health] = 0;
            //Reload scene
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if(!isDead)
            {
                isDead = true;
                EventManager.TriggerEvent(Event.PlayerDeath, null);
            }
        }
        if(Input.GetKeyDown(KeyCode.L)) 
        {
            hasBeenInit = false;
        }
    }

    bool IsPlayerDead()
    {
        return cachedCalculatedValues[Stat.Current_Health] <= 0;
    }


    private void OnEnemyHit(IEventPacket packet)
    {
        EnemyHitPacket ehp = packet as EnemyHitPacket;
        if (!PlayerController.Instance.invulnerability)
        {
            float enemyDamage = ehp.healthDeplete;
            enemyDamage = (cachedCalculatedValues[Stat.Enemy_Dmg]) / 100.0f * enemyDamage;
            WoundPlayer((int)enemyDamage, false);
            //cachedCalculatedValues[Stat.Current_Health] -= (int)ehp.healthDeplete;
            PlayerController.Instance.invulnerability = true;
            EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
            {
                textColor = Color.red,
                position = PlayerController.Instance.transform.position,
                damage = (int)enemyDamage
            });
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Event.EnemyHitPlayer, OnEnemyHit);
        EventManager.StopListening(Event.PlayerDeath, OnPlayerDeath);
        SaveValues();
    }

    public void SaveValues()
    {
        foreach(var kvp in baseStatValues)
        {
            PlayerPrefs.SetFloat(kvp.Key.ToString(), kvp.Value);
        }
        PlayerPrefs.SetString("isSet", hasBeenInit.ToString());
    }

    public void LoadValues()
    {
        baseStatValues = new Dictionary<Stat, float>();
        cachedCalculatedValues = new Dictionary<Stat, float>();
        var values = Enum.GetValues(typeof(Stat));
        foreach(var stat in values)
        {
            baseStatValues.Add((Stat)stat, PlayerPrefs.GetFloat(stat.ToString()));
            cachedCalculatedValues.Add((Stat)stat, PlayerPrefs.GetFloat(stat.ToString()));

        }

        //maxHealth =  PlayerPrefs.GetInt(MaxStat.MaxHealth.ToString(), maxHealth);
        //maxDamage = PlayerPrefs.GetInt(MaxStat.MaxDamage.ToString(), maxDamage);
        //maxDefence = PlayerPrefs.GetFloat(MaxStat.MaxDefence.ToString(), maxDefence);
        //maxDexterity = PlayerPrefs.GetFloat(MaxStat.MaxDexterity.ToString(), maxDexterity);
        //maxSpeed = PlayerPrefs.GetFloat(MaxStat.MaxSpeed.ToString(), maxSpeed);
        hasBeenInit = Convert.ToBoolean(PlayerPrefs.GetString("isSet"));
    }

    public void ResetStatValues()
    {
        //currentHealth = maxHealth = baseHealth;
        //currentDamage = maxDamage = baseDamage;
        //currentSpeed = maxSpeed = baseSpeed;
        //currentDexterity = currentDexterity = baseDexterity;
        //currentDefence = maxDefence = baseDefence;
        baseStatValues.Clear();
        cachedCalculatedValues.Clear();
        foreach(var sv in baseValuesObject.statValues)
        {
            baseStatValues.Add(sv.stat, sv.value);
            cachedCalculatedValues.Add(sv.stat, sv.value);
        }
    }

    private void OnPlayerDeath(IEventPacket packet)
    {
        //currentHealth = maxHealth;
        if(PlayerPrefs.HasKey(PlayerPrefsKeys.DEATH_NUMBER.ToString()))
        {
            int value = PlayerPrefs.GetInt(PlayerPrefsKeys.DEATH_NUMBER.ToString());
            PlayerPrefs.SetInt(PlayerPrefsKeys.DEATH_NUMBER.ToString(), value + 1);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.DEATH_NUMBER.ToString(), 1);
        }

    }

    public void WoundPlayer(int amount, bool isDefence)
    {
        if(isDefence)
        {

        }
        else
        {
            cachedCalculatedValues[Stat.Current_Health] -= amount;
            baseStatValues[Stat.Current_Health] -= amount;
        }
    }

    public void HealPlayer(float amount)
    {
        amount = (int)((cachedCalculatedValues[Stat.Healing] / 100.0f) * amount);
        cachedCalculatedValues[Stat.Current_Health] += amount;
    }

    public void AddModifier(StatModifier modifier)
    {
        statModifiers.Add(modifier);
        CalculateStat(modifier.stat);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        statModifiers.Remove(modifier);
        CalculateStat(modifier.stat);
    }

    public void CalculateStat(Stat stat)
    {
        List<StatModifier> modifiers = statModifiers.Where(x => x.stat == stat).ToList();
        List<StatModifier> setModifiers = modifiers.Where(x => x.statModifierType == StatModifierType.SET).ToList();
        float numerical = 0;
        float percentage = 0;
        if(setModifiers.Count != 0)
        {
            cachedCalculatedValues[stat] = setModifiers[0].modifierValue;
            EventManager.TriggerEvent(Event.StatChanged, new StatChangedPacket()
            {
                stat = stat
            });
            return;
        }
        else
        {
            foreach(StatModifier modifier in modifiers)
            {
                if (modifier.statModifierType == StatModifierType.PERCENTAGE)
                    percentage += modifier.modifierValue;
                else if (modifier.statModifierType == StatModifierType.NUMERICAL)
                    numerical += modifier.modifierValue;
            }

            percentage = Mathf.Clamp(percentage, -100.0f, 1000.0f);

            cachedCalculatedValues[stat] = (baseStatValues[stat] + numerical) * (100.0f + percentage)/100.0f;
            EventManager.TriggerEvent(Event.StatChanged, new StatChangedPacket()
            {
                stat = stat
            });
        }
    }

    public float GetCurrentHealthPercentage()
    {
        return cachedCalculatedValues[Stat.Current_Health] / cachedCalculatedValues[Stat.Health];
    }

    public void ResetDeath()
    {
        //currentHealth = maxHealth;
        cachedCalculatedValues[Stat.Current_Health] = baseStatValues[Stat.Health];
        isDead = false;
    }
}