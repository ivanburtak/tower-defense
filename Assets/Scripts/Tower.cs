using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tower : MonoBehaviour
{
    public TowerData data;
    private Enemy target;
    private float fireCountdown = 0f;

    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    void UpdateTarget()
    {
        Enemy best = null;
        int bestIndex = -1;
        float bestDistance = 0;

        foreach (var enemy in WaveManager.Instance.ActiveEnemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) > data.range)
                continue;

            if (enemy.PathIndex < bestIndex) continue;

            // if (data.slowAmount > 0f && enemy.data.isImmuneToSlow) continue;

            float distance = Vector3.Distance(enemy.transform.position, Waypoints.Path[enemy.PathIndex].transform.position);
            if (enemy.PathIndex == bestIndex)
            {
                if (distance < bestDistance)
                {
                    best = enemy;
                    bestDistance = distance;
                }
                continue;
            }
            // e.PathIndex > bestIndex
            best = enemy;
            bestIndex = enemy.PathIndex;
            bestDistance = distance;
        }

        target = best;
    }

    void Update()
    {
        if (target == null) return;

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / data.fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject obj = ProjectilePool.Instance.Get(transform.position);
        Projectile projectile = obj.GetComponent<Projectile>();
        projectile.Initialise(target, data.projectileSpeed, data.damage, data.aoeRadius, data.slowAmount, data.projectileSprite);
    }
}