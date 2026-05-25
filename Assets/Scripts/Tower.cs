using UnityEngine;

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

        float step = data.rotationSpeed * Time.deltaTime;
        Vector3 dir = target.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), step);

        float angleDiff = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);

        if (fireCountdown <= 0f && Mathf.Abs(angleDiff) < 5f)
        {
            Shoot();
            fireCountdown = 1f / data.fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject obj = ProjectilePool.Instance.Get(data.projectilePrefab, transform.position);
        Projectile projectile = obj.GetComponent<Projectile>();
        projectile.Initialise(target, data.projectileSpeed, data.damage, data.aoeRadius, data.slowAmount, data.projectilePrefab);
    }
}