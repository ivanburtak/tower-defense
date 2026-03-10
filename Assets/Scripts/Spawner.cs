using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private float timeBetweenWaves = 2f;

    private float countdown = 0f;

    public static List<Enemy> enemies
    {
        get;
        private set;
    }

    void Awake()
    {
        Instance = this;
        enemies = new List<Enemy>();
        InvokeRepeating("UpdateEnemyProgress", 0f, 0.5f);
    }

    void Update()
    {
        if (countdown <= 0f)
        {
            SpawnEnemy();
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
    }

    void SpawnEnemy()
    {
        Transform enemyTransform = Instantiate(enemyPrefab, transform.position, transform.rotation);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        enemies.Add(enemy);
    }

    public void DestroyEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
        // So towers don't shoot at non existing enemy
        UpdateEnemyProgress();
    }

    public void UpdateEnemyProgress()
    {
        enemies.Sort((a, b) =>
        {
            int pathCompare = a.pathIndex.CompareTo(b.pathIndex);
            if (pathCompare != 0)
                return pathCompare;

            return a.GetDistanceToWaypoint().CompareTo(b.GetDistanceToWaypoint());
        });
    }
}
