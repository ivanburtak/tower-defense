using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private float speed;
    private int damage;
    private Vector3 targetPosition;
    public void Initialise(Enemy target, float speed, int damage)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
    }

    void MoveTowardsEnemyPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    void Update()
    {
        if (target == null)
        {
            // Test: Possible edgecase of target being null before the first update?
            // if (targetPosition == null)
            // {
            //     Destroy(gameObject);
            //     return;
            // }

            // It would be stupid if projectile just disappears, instead home at the target's last position
            MoveTowardsEnemyPosition();

            if (transform.position == targetPosition)
                Destroy(gameObject);

            return;
        }

        targetPosition = target.transform.position;
        MoveTowardsEnemyPosition();

        if (transform.position == targetPosition)
        {
            Destroy(gameObject);
            target.GetHit(damage);
        }
    }
}
