using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, ICollectible
{
    public int healthToRestore;

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(healthToRestore);
        Destroy(gameObject);
    }
}
//source:https://youtube.com/watch?v=qREiQ5vSAng&list=PLgXA5L5ma2Bveih0btJV58REE2mzfQLOQ&index=6