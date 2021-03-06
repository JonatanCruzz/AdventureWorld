using System;
using System.Collections.Generic;
using AdventureWorld.Prueba.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public List<BaseEnemy> enemies;
    [ReadOnly] public List<GenericEnemy> spawnedEnemies;
    [ReadOnly] public List<GenericEnemy> deadEnemies;

    public List<Transform> spawnPoints;
    public float spawnCooldown = 0f;
    public float spawnCooldownMax = 1f;
    public float spawnCooldownMin = 0.5f;

    public int maxEnemies = 10;

    public void Start()
    {
        spawnCooldown = Random.Range(spawnCooldownMin, spawnCooldownMax);
    }

    public void Update()
    {
        if (spawnCooldown > 0)
        {
            spawnCooldown -= Time.deltaTime;
        }
        else
        {
            spawnCooldown = Random.Range(spawnCooldownMin, spawnCooldownMax);
            SpawnEnemy();
        }
    }


    public void SpawnEnemy()
    {
        if (spawnedEnemies.Count >= maxEnemies) return;
        var enemyData = enemies[Random.Range(0, enemies.Count)];
        var enemy = Instantiate(enemyData.prefab, spawnPoints[Random.Range(0, spawnPoints.Count)].position,
            Quaternion.identity);
        enemy.name = enemyData.name;
        enemy.transform.parent = transform;
        var enemyScript = enemy.AddComponent<GenericEnemy>();
        enemyScript.baseEnemy = enemyData;
        enemyScript.spawner = this;
        // enemyScript.Init();  
        // enemyScript.baseEnemy = enemyData;
        spawnedEnemies.Add(enemyScript);
    }

    public void RemoveEnemy(GenericEnemy enemy)
    {
        spawnedEnemies.Remove(enemy);
        Destroy(enemy.gameObject);
        // spawn coin at enemy position
        var coin = Instantiate(coinPrefab, enemy.transform.position, Quaternion.identity);

        var coinScript = coin.AddComponent<GoldCoin>();
        coinScript.value = Mathf.RoundToInt(Random.Range(enemy.baseEnemy.MinMaxCoins.x, enemy.baseEnemy.MinMaxCoins.y));
        
    }
}