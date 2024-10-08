using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : WeaponController
{
    SpriteRenderer sr;

    protected override void Start()
    {
        base.Start();
        sr = GetComponentInParent<SpriteRenderer>();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedLaser = Instantiate(weaponData.Prefab);

        // Check if the player sprite is flipped
        float xOffset = sr.flipX ? -1 : 1 ;

        if (xOffset < 0)
        {
            spawnedLaser.transform.position = transform.position + new Vector3(xOffset, 0f, 0f);  // If the player is flipped, offset the laser's spawnpoint so it is at the end of the barrel 
        }
        else
        {
            spawnedLaser.transform.position = transform.position;
        }


        spawnedLaser.GetComponent<LaserBehavior>().DirectionChecker(pm.lastMovedVector);
    }
}
