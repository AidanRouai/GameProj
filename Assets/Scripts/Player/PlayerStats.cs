using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public CharacterScriptableObject characterData;

    //Initialize current stats
    public float currentHealth;
    public float currentRecovery;
    public float currentMoveSpeed;
    public float currentStrength;
    public float currentProjectileSpeed;
    public float currentMagnet; 

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

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    public GameObject firstPassiveItemTest, secondPassiveItemTest;
    public GameObject secondWeaponTest; 
    private void Awake()
    {
        inventory = GetComponent<InventoryManager>();

        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentStrength = characterData.Strength;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;

        SpawnPassiveItem(firstPassiveItemTest);
        SpawnPassiveItem(secondPassiveItemTest);
        SpawnWeapon(secondWeaponTest);
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

        Recover();
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

    public void IncreaseProjectileSpeed(float amount)
    {
        currentProjectileSpeed *= amount;
    }

    void Recover()
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;

            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth; //Player's health won't exceed MaxHealth 
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if (weaponIndex>= inventory.weaponSlots.Count - 1) //List starts at 0 
        {
            Debug.LogError("Inventory slots already full");
            return; 
        }

        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); //Set weapon to be child of the player 
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>()); //Add weapon to inventory slot 

        weaponIndex++; //So that the next weapon is added to the next weapon slot 

    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1) //List starts at 0 
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform); //Set item to be child of the player 
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); //Add weapon to inventory slot 

        passiveItemIndex++; //So that the next passiveItem is added to the next weapon slot 

    }
}
