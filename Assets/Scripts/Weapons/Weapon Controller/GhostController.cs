using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : WeaponController
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
        GameObject spawnedGhost = Instantiate(weaponData.Prefab);

        float xOffset = sr.flipX ? -1 : 1;

        if (xOffset < 0)
        {
            spawnedGhost.transform.position = transform.position + new Vector3(xOffset, 0f, 0f);  // If the player is flipped, offset the laser's spawnpoint so it is at the end of the barrel 
        }
        else
        {
            spawnedGhost.transform.position = transform.position;
        }


        spawnedGhost.GetComponent<GhostBehavior>().DirectionChecker(pm.lastMovedVector);
    }
}
