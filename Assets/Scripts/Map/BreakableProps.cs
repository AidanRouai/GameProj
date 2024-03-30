using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public float health; 

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
//source:https://youtube.com/watch?v=qREiQ5vSAng&list=PLgXA5L5ma2Bveih0btJV58REE2mzfQLOQ&index=6