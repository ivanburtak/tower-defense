using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform fireTransform;

    [Header("Attributes")]
    [SerializeField] private float range = 20f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotationSpeed = 180f; // degrees per second
    [SerializeField] private float projectileSpeed = 30f;
    [SerializeField] private int projectileDamage = 1;


    private Enemy target;
    private float fireCountdown = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        for (int i = Spawner.enemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = Spawner.enemies[i];

            // Enemies are already sorted from furtherest to closest to the base
            // If we iterate backwards we always get the closest enemy
            // The only question is whether the enemy is within the tower's range
            if (Vector3.Distance(enemy.transform.position, transform.position) <= range)
            {
                target = enemy;
                return;
            }
        }

        target = null;
    }

    void Update()
    {
        if (target == null)
            return;

        float step = rotationSpeed * Time.deltaTime;

        Vector3 dir = target.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), step);


        float angleDiff = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);
        float tolerance = 5f;

        if (fireCountdown <= 0f && Mathf.Abs(angleDiff) < tolerance)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject obj = Instantiate(projectilePrefab, fireTransform.position, fireTransform.rotation);
        Projectile projectile = obj.GetComponent<Projectile>();
        projectile.Initialise(target, projectileSpeed, projectileDamage);
    }

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, range);
    // }
}
