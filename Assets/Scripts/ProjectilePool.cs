using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    private readonly Dictionary<GameObject, Queue<GameObject>> pools = new();

    void Awake() => Instance = this;

    public GameObject Get(GameObject prefab, Vector3 pos)
    {
        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        Queue<GameObject> pool = pools[prefab];
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab);
        obj.transform.SetPositionAndRotation(pos, Quaternion.identity);
        obj.SetActive(true);
        return obj;
    }

    public void Return(Projectile projectile, GameObject prefab)
    {
        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        projectile.gameObject.SetActive(false);
        pools[prefab].Enqueue(projectile.gameObject);
    }
}