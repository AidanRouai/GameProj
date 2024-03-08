using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;

    void Start()
    {
        SpawnProps();
    }

    
    void SpawnProps()
    {
        //Spawn a random prop at every Prop Location Spawnpoint
        foreach (GameObject sp in propSpawnPoints)
        {
            int rand = Random.Range(0, propPrefabs.Count);
            GameObject prop = Instantiate(propPrefabs[rand], sp.transform.position, Quaternion.identity);
            prop.transform.parent = sp.transform; //Move spawned object into map 

        }
    }
}

//Source: https://www.youtube.com/watch?v=QN8dm0RD3mY&list=PLgXA5L5ma2Bveih0btJV58REE2mzfQLOQ&index=24