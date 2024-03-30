using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed; 

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        playerCollector.radius = player.CurrentMagnet;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out ICollectible collectible)) // Check if the other game object is collectible (has the ICollectible interface)
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (transform.position - col.transform.position).normalized;
            rb.AddForce(forceDirection * pullSpeed);

            collectible.Collect(); //If yes, calls the collect method
        }
    }
}
//source:https://youtube.com/watch?v=qREiQ5vSAng&list=PLgXA5L5ma2Bveih0btJV58REE2mzfQLOQ&index=6