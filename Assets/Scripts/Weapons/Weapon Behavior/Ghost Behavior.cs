using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : ProjectileWeaponBehaviour
{
    protected override void Start()
    {
        base.Start();
        direction = Random.insideUnitCircle.normalized;

    }

    void Update()
    {
        // Move the projectile
        transform.position += direction * currentSpeed * Time.deltaTime;

        // Get the screen bounds in world coordinates
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Get the projectile's position
        Vector3 position = transform.position;

        // Check if the projectile hits the screen edges
        if (position.x > screenBounds.x || position.x < -screenBounds.x)
        {
            // Reverse X direction
            direction.x *= -1;
        }
        if (position.y > screenBounds.y || position.y < -screenBounds.y)
        {
            // Reverse Y direction
            direction.y *= -1;
        }
    }
}
