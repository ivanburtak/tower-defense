using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance { get; private set; }
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 60;

    private Queue<GameObject> pool = new();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get(Vector3 pos)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(enemyPrefab);
        obj.transform.position = pos;
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}