using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemymovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;
    SpriteRenderer sr; 

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
        sr = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime); //Moves the enemy towards the player
        
        if (transform.position.x < player.transform.position.x) //If the player is to the right of the sprite
        {
            sr.flipX= false; 
        }
        else
        {
            sr.flipX = true;
        }
    }

}
