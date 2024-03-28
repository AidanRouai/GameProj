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

        // Clamp the projectile's position to stay within the screen bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -screenBounds.x, screenBounds.x),
            Mathf.Clamp(transform.position.y, -screenBounds.y, screenBounds.y),
            transform.position.z
        );

        // Check if the projectile hits the screen edges and reverse direction if necessary
        if (Mathf.Abs(transform.position.x) >= screenBounds.x)
        {
            // Reverse X direction
            direction.x *= -1;
        }
        if (Mathf.Abs(transform.position.y) >= screenBounds.y)
        {
            // Reverse Y direction
            direction.y *= -1;
        }
    }
}
