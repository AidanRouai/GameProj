using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{

    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate; 

    }

    public List<Drops> drops;

    void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }

        float randomNumber = UnityEngine.Random.Range(0f, 100f); //Percentage chance that the item will drop
        List<Drops> possibleDrops = new List<Drops>();

        foreach (Drops rate in drops)
        {
            if (randomNumber <= rate.dropRate)
            {
               possibleDrops.Add(rate);
            }

            if (possibleDrops.Count > 0) //See if drops are possible
            {
                Drops drops = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
                Instantiate(drops.itemPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
