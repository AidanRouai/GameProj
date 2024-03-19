using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : ScriptableObject
{
    [SerializeField]
    GameObject startingWeapon;

    public GameObject StartingWeapon { get => startingWeapon; private set => startingWeapon = value;}

    [SerializeField]
    GameObject maxHealth;

    public GameObject MaxHealth { get => maxHealth; private set => maxHealth = value; }


}
