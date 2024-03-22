using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickup, ICollectible
{
    public int experienceGranted;
    public void Collect()
    {
        Debug.Log("Called");
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
}
//source:https://youtube.com/watch?v=qREiQ5vSAng&list=PLgXA5L5ma2Bveih0btJV58REE2mzfQLOQ&index=6