using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public float timeBetweenWaves = 2f;

    public Base destination;
    private float countdown = 0f;

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
        enemy.Initialise(destination);
    }
}
