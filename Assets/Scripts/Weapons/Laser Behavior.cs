using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : ProjectileWeaponBehaviour
{

    LaserController lc;

    protected override void Start()
    {
        base.Start();
        lc = FindObjectOfType<LaserController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * lc.speed * Time.deltaTime; //Set the movement of the laser  
    }
}
