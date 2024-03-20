using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out ICollectible collectible)) // Check if the other game object is collectible (has the ICollectible interface)
        {
            collectible.Collect(); //If yes, calls the collect method
        }
    }
}
