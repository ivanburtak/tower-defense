using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public float timeBetweenWaves = 2f;
    private float countdown = 0f;

    void Update()
    {
        if (countdown <= 0)
        {
            SpawnEnemy();
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }
}
