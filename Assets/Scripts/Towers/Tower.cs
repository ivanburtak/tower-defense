using UnityEngine;
using System.Collections.Generic;

namespace TowerDefense.Towers
{
    /// <summary>
    /// Base abstract class for all tower types. Handles targeting, rotation, and attack logic.
    /// </summary>
    public abstract class Tower : MonoBehaviour
    {
        [SerializeField] protected float attackRange = 5f;
        [SerializeField] protected float attackCooldown = 1f;
        [SerializeField] protected int damage = 10;
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected Transform firePoint;
        [SerializeField] protected float projectileSpeed = 10f;

        protected float lastAttackTime = 0f;
        protected Enemy currentTarget;
        protected CircleCollider2D rangeCollider;

        protected virtual void Start()
        {
            // Create range collider for detection
            rangeCollider = gameObject.AddComponent<CircleCollider2D>();
            rangeCollider.radius = attackRange;
            rangeCollider.isTrigger = true;

            if (firePoint == null)
                firePoint = transform;
        }

        protected virtual void Update()
        {
            if (Core.GameManager.Instance.IsGameOver() || Core.GameManager.Instance.IsGameWon())
                return;

            FindTarget();
            RotateTowardsTarget();
            AttemptAttack();
        }

        /// <summary>
        /// Find the closest enemy within range.
        /// </summary>
        protected virtual void FindTarget()
        {
            Enemy[] allEnemies = FindObjectsOfType<Enemy>();
            currentTarget = null;
            float closestDistance = attackRange + 1f;

            foreach (Enemy enemy in allEnemies)
            {
                if (enemy == null || enemy.IsDead()) continue;

                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    currentTarget = enemy;
                }
            }
        }

        /// <summary>
        /// Rotate the tower towards the current target.
        /// </summary>
        protected virtual void RotateTowardsTarget()
        {
            if (currentTarget == null) return;

            Vector2 direction = (currentTarget.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        /// <summary>
        /// Attempt to attack the current target if cooldown is ready.
        /// </summary>
        protected virtual void AttemptAttack()
        {
            if (currentTarget == null || Time.time < lastAttackTime + attackCooldown)
                return;

            Attack();
            lastAttackTime = Time.time;
        }

        /// <summary>
        /// Fire a projectile at the current target.
        /// </summary>
        protected virtual void Attack()
        {
            if (projectilePrefab == null || currentTarget == null)
                return;

            GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Projectile projectile = projectileObj.GetComponent<Projectile>();

            if (projectile != null)
            {
                projectile.Initialize(currentTarget, damage, projectileSpeed);
            }

            OnAttack();
        }

        /// <summary>
        /// Override this method for tower-specific attack behavior (sound, animation, etc).
        /// </summary>
        protected virtual void OnAttack()
        {
            // Can be overridden in derived classes
        }

        public abstract string GetTowerName();
        public abstract int GetCost();
        public int GetDamage() => damage;
        public float GetRange() => attackRange;
    }
}
