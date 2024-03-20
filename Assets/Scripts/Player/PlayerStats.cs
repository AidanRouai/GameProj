using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public CharacterScriptableObject characterData;

    //Initialize current stats
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentStrength;
    float currentProjectileSpeed;

    //Experience and level of player
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceLevelCap;

    [System.Serializable] //Allows these fields to be changeable in the Unity editor

    public class LevelRange
    {
        public int startLevel; 
        public int endLevel;
        public int experienceCapIncrease;
    }


    [Header("Invincibility frames/I frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    private void Awake()
    {
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentStrength = characterData.Strength;
        currentProjectileSpeed = characterData.ProjectileSpeed;
    }

    public void Start()
    {
        experienceLevelCap = levelRanges[0].experienceCapIncrease;
    }

    private void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }

        else if (isInvincible)
        {
            isInvincible = false;
        }
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
    }

    void LevelUpChecker()
    {
     if (experience >= experienceLevelCap)
        {
            level++;
            experience -= experienceLevelCap;

            int experienceCapIncrease = 0; 
            foreach (LevelRange range in levelRanges) 
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible) //If player not in I frame, take damage
        {
            currentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0)
            {
                Kill();
            }
        }  

        
    }

    public void Kill()
    {
        Debug.Log("YOU ARE DEAD");
    }

    public void RestoreHealth(float amount)
    {
    if ( currentHealth < characterData.MaxHealth) //Heal if player is not at max health 
        {
            currentHealth += amount;

            if (currentHealth > characterData.MaxHealth) //Make sure to not exceed max health 
            {
                currentHealth = characterData.MaxHealth;
            }
        }
  
    }
}
