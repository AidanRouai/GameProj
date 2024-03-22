using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota; //Total number of enemies that should spawn per wave 
        public float spawnInterval; //The interaval at which enemies spawn 
        public int spawnCount; //The number of enemies that already have spawned in this wave 
    }

    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; //The number of enemies to spawn this wave 
        public int spawnCount; //Number of enemies of this type that have already spawned in this wave 
        public GameObject enemyPrefabs;
    }

    public List <Wave> waves; //A list of all the waves in the game 
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

//source: https://www.youtube.com/watch?v=h2cg4ucDuWw&list=PLgXA5L5ma2Bveih0btJV58REE2mzfQLOQ&index=9
