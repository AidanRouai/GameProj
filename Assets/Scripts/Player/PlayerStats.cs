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
    float currentMagnet;


    #region Current Stats Properties 
    public float CurrentHealth
    {
        get { return currentHealth;  }
        set
        {
            if (currentHealth != value) //check if the value has changed
            {
                currentHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health:" + currentHealth;
                }
                //Update the value of the stat in real time 
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery;  }
        set
        {
            if (currentRecovery != value) //check if the value has changed
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery:" + currentRecovery;
                }
                //Update the value of the stat in real time 
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed;  }
        set
        {
            if (currentMoveSpeed != value) //check if the value has changed
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed:" + currentMoveSpeed;
                }
                //Update the value of the stat in real time 
            }
        }
    }

    public float CurrentStrength
    {
        get { return currentStrength;  }
        set
        {
            if (currentStrength != value) //check if the value has changed
            {
                currentStrength = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentStrengthDisplay.text = "Strength:" + currentStrength;
                }
                //Update the value of the stat in real time 
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed;  }
        set
        {
            if (currentProjectileSpeed != value) //check if the value has changed
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed:" + currentProjectileSpeed;
                }
                //Update the value of the stat in real time 
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet;  }
        set
        {
            if (currentMagnet != value) //check if the value has changed
            {
                currentMagnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet:" + currentMagnet;
                }
                //Update the value of the stat in real time 
            }
        }
    }
    #endregion

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

    public GameObject firstPassiveItemTest, secondPassiveItemTest, thirdPassiveItemTest, fourthPassiveItemTest;
    public GameObject secondWeaponTest;
    public GameObject laserWeapon; 
    private void Awake()
    {
        inventory = GetComponent<InventoryManager>();
        

        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentStrength = characterData.Strength;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;

        SpawnWeapon(laserWeapon);

        SpawnPassiveItem(firstPassiveItemTest);
        SpawnPassiveItem(secondPassiveItemTest);
        SpawnPassiveItem(thirdPassiveItemTest);
        SpawnPassiveItem(fourthPassiveItemTest);
        SpawnWeapon(secondWeaponTest);
    }

    public void Start()
    {
        experienceLevelCap = levelRanges[0].experienceCapIncrease;

        GameManager.instance.currentHealthDisplay.text = "Health:" + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery:" + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed:" + currentMoveSpeed;
        GameManager.instance.currentStrengthDisplay.text = "Strength:" + currentStrength;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed:" + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet:" + currentMagnet;

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
            CurrentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }
        }  

        
    }

    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReached(level);
            GameManager.instance.AssignChosenItemsUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount)
    {
    if ( CurrentHealth < characterData.MaxHealth) //Heal if player is not at max health 
        {
            CurrentHealth += amount;

            if (CurrentHealth > characterData.MaxHealth) //Make sure to not exceed max health 
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
  
    }

    public void IncreaseProjectileSpeed(float amount)
    {
        CurrentProjectileSpeed *= amount;
    }

    void Recover()
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            if(CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth; //Player's health won't exceed MaxHealth 
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
