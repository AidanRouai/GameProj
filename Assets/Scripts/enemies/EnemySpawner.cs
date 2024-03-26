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

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; //The number of enemies to spawn this wave 
        public int spawnCount; //Number of enemies of this type that have already spawned in this wave 
        public GameObject enemyPrefabs;
    }

    public List <Wave> waves; //A list of all the waves in the game 
    public int currentWaveCount; //Index of current wave 

    [Header("Spawn Attributes")]
    float spawnTimer; //When to spawn next enemy
    public int enemiesAlive;
    public int maxEnemiesAllowed; //Max number of enemies allowed on the map 
    public bool maxEnemiesReached = false; //Indicates if the maximum amount of enemies on the map has been reached 
    public float waveInterval; 

    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    void Update()
    {

        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0) //Check if current wave has ended and if next wave should start 
        {
            StartCoroutine(BeginNextWave());
        }
        spawnTimer += Time.deltaTime;

        if(spawnTimer >= waves[currentWaveCount].spawnInterval) //If it's time to spawn next enemy
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval); //Wait for waveInterval seconds before next wave 

        if (currentWaveCount < waves.Count - 1) //If there are more waves to spawn, spawn next wave 
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0; 
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;

    }

    /*Summary
     * This method will stop spawning enemies if we have reached the maximum amount of enemies already 
     * The method will only spawn enemies in a particular wave until its time for the next wave 
     */
    void SpawnEnemies()
    {
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached) //if the minimum number of enemies have spawned 
        {
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups) //spawn each group of enemies till the wave quota is satisfied
            {
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)//if the minimum number of enemies of this type have spawned
                {
                   if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return; 
                    }

                    Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
                    Instantiate(enemyGroup.enemyPrefabs, spawnPosition, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++; 
                }
            }
        }

        if(enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false; 
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}

//source: https://www.youtube.com/watch?v=h2cg4ucDuWw&list=PLgXA5L5ma2Bveih0btJV58REE2mzfQLOQ&index=9
