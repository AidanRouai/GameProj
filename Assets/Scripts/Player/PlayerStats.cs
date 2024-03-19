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
}
