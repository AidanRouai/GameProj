using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedLaser = Instantiate(prefab);
        spawnedLaser.transform.position = transform.position; //Assign the position to be this object which is parented to the player 
        spawnedLaser.GetComponent<LaserBehavior>().DirectionChecker(pm.lastMovedVector);
    }
}
