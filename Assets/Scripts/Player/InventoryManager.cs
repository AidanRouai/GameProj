using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(4);
    public int[] weaponLevels = new int[4];
    public List<Image> weaponUISlots = new List<Image>(4);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(4);
    public int[] passiveItemLevels = new int[4];
    public List<Image> passiveItemUISlots = new List<Image>(4);


    [System.Serializable]
    public class WeaponUpgrade
    {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject PassiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public Text upgradeNameDisplay;
        public Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton; 
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();       
    }

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true; //Enables the image component
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled = true; //Enables the image component
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;
        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogError("No next level for" + weapon.name);
            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform); 
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level; //make sure we have the correct item level

            weaponUpgradeOptions[upgradeIndex].weaponData = upgradedWeapon.GetComponent<WeaponController>().weaponData;

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        PassiveItem passiveItem = passiveItemSlots[slotIndex];
        if (!passiveItem.passiveItemData.NextLevelPrefab) //To check if there is a next level for the current item 
        {
            Debug.LogError("No next level for" + passiveItem.name);
        }
        GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
        upgradedPassiveItem.transform.SetParent(transform); //Set the item to be a child of the player
        AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
        Destroy(passiveItem.gameObject);
        weaponLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level;

        passiveItemUpgradeOptions[upgradeIndex].PassiveItemData = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    void ApplyUpgradeOptions()
    {
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgrade = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);


        foreach (var upgradeOption in upgradeUIOptions)
        {


             int upgradeType = Random.Range(1, 3);

            if (upgradeType == 1)
            {
                WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];

                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

                if (chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool newWeapon = false; 
                    for(int i = 0; i < weaponSlots.Count; i++)
                    {
                        if(weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                if (!chosenWeaponUpgrade.weaponData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex)); //Clicking the button levels up the shown weapon 
                                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if (newWeapon) //Spawn a new weapon
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            }
            else if (upgradeType == 2)
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrade[Random.Range(0, availablePassiveItemUpgrade.Count)];

                availablePassiveItemUpgrade.Remove(chosenPassiveItemUpgrade);

                if (chosenPassiveItemUpgrade != null)
                {

                    EnableUpgradeUI(upgradeOption);


                    bool newPassiveItem = false;
                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.PassiveItemData)
                        {
                            newPassiveItem = false;
                            if (!newPassiveItem)
                            {
                                if (!chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.passiveItemUpgradeIndex)); //Clicking the button levels up the shown weapon 
                                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newPassiveItem = true;
                        }
                    }
                    if (newPassiveItem) //Spawn a new weapon
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.Name;
                    }
                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.PassiveItemData.Icon;

                }
            }
        }
    }

    void RemoveUpgradeOption()
    {
        foreach(var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption); //Disables all ui options before applying the upgrades to them 
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOption();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
}

//source:https://www.youtube.com/watch?v=HtZYoZ900y8
