using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base script of all projectile behaviours [To be placed on a prefab of a weapon that is a projectile]
/// </summary>
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    protected Vector3 direction;

    public float destroyAfterSeconds;

    //Current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;



        if (dirx < 0 && diry ==0) //left
        {
            scale.x = scale.x * -1; 
        }
        else if (dirx == 0 && diry < 0) //down
        {           
            rotation.z = -90f;
        }
        else if (dirx == 0 && diry > 0) //up
        {
            rotation.z = 90f;
        }
        else if (dirx > 0 && diry > 0) //right up
        {
            rotation.z = 45f;
        }
        else if (dirx > 0 && diry < 0) //right down
        {
            rotation.z = -45f;
        }
        else if (dirx < 0 && diry > 0) //left up
        {
            rotation.z = 135;
        }
        else if (dirx < 0 && diry < 0) //left down
        {
            rotation.z = 225f;
        }
        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        //Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
            ReducePierce();
        }

        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(currentDamage);
                ReducePierce();
            }
        }
    }
    void ReducePierce() //Destroy once pierce hits 0
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}