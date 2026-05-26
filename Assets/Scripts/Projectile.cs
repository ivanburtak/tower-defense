using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private float speed;
    private int damage;
    private float aoeRadius;
    private float slowAmount;
    private Vector3 targetPosition;

    public void Initialise(Enemy target, float speed, int damage, float aoeRadius, float slowAmount, Sprite sprite)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.aoeRadius = aoeRadius;
        this.slowAmount = slowAmount;
        targetPosition = target != null ? target.transform.position : Vector3.zero;
        GetComponent<SpriteRenderer>().sprite = sprite;
        RotateTowards(targetPosition);
    }

    void RotateTowards(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void ReturnToPool()
    {
        ProjectilePool.Instance.Return(gameObject);
    }

    void MoveTowardsEnemyPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void HitSingle(Enemy enemy)
    {
        enemy.GetHit(damage);
        if (slowAmount > 0f)
            enemy.ApplySlow(slowAmount, 2f);
    }

    void Explode()
    {
        var enemiesSnapshot = WaveManager.Instance.ActiveEnemies.ToArray();
        foreach (var enemy in enemiesSnapshot)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) <= aoeRadius)
                HitSingle(enemy);
        }
    }

    void Update()
    {
        if (target == null)
        {
            MoveTowardsEnemyPosition();
            if (transform.position == targetPosition)
            {
                if (aoeRadius > 0f) Explode();
                ReturnToPool();
            }
            return;
        }

        targetPosition = target.transform.position;
        RotateTowards(targetPosition);
        MoveTowardsEnemyPosition();

        if (transform.position == targetPosition)
        {
            if (aoeRadius > 0f) Explode();
            else HitSingle(target);
            ReturnToPool();
        }
    }
}