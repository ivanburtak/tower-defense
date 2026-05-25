using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [SerializeField] private EnemyData[] enemyTypes;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private int baseAttackBudget = 200;

    public List<Enemy> ActiveEnemies { get; private set; } = new();

    void Awake() => Instance = this;

    public void StartWave(int round)
    {
        int budget = baseAttackBudget + (round - 1) * 50;
        List<EnemyData> wave = BuildWave(budget);
        StartCoroutine(SpawnWave(wave));
    }

    List<EnemyData> BuildWave(int budget)
    {
        List<EnemyData> wave = new();
        int spent = 0;

        while (spent < budget && wave.Count < 50)
        {
            var affordable = System.Array.FindAll(enemyTypes, e => e.cost <= budget - spent);
            if (affordable.Length == 0) break;
            EnemyData pick = affordable[Random.Range(0, affordable.Length)];
            wave.Add(pick);
            spent += pick.cost;
        }
        return wave;
    }

    IEnumerator SpawnWave(List<EnemyData> wave)
    {
        foreach (EnemyData data in wave)
        {
            SpawnEnemy(data);
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitUntil(() => ActiveEnemies.Count == 0);
        GameManager.Instance.OnWaveComplete();
    }

    void SpawnEnemy(EnemyData data)
    {
        GameObject obj = EnemyPool.Instance.Get(data.prefab, spawnLocation.transform.position);
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.ResetTo(data);
        ActiveEnemies.Add(enemy);
    }

    public void OnEnemyDied(Enemy enemy)
    {
        ActiveEnemies.Remove(enemy);
        EnemyPool.Instance.Return(enemy.gameObject, enemy.data.prefab);
    }
}