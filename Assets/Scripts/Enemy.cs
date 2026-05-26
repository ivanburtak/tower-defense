using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;

    private float currentSpeed;
    private int currentHealth;
    private Transform target;
    private float slowTimer;
    private bool isSlowed;

    public int PathIndex { get; private set; } = 0;

    [SerializeField] private SpriteRenderer spriteRenderer;

    public void ResetTo(EnemyData data)
    {
        this.data = data;
        currentSpeed = data.speed;
        currentHealth = data.health;
        slowTimer = 0f;
        isSlowed = false;

        PathIndex = 0;
        target = Waypoints.Path[0];
        UpdateSprite();
    }

    void Update()
    {

        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;

            if (slowTimer <= 0f)
            {
                currentSpeed = data.speed;
                isSlowed = false;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);

        if (transform.position == target.position)
            NextWaypoint();

        spriteRenderer.sortingOrder = -(int)(transform.position.y * 10);
    }

    void NextWaypoint()
    {
        PathIndex++;
        if (PathIndex == Waypoints.Path.Length)
        {
            Base.Instance.GetHit(data.damage);
            WaveManager.Instance.OnEnemyDied(this);
            return;
        }
        target = Waypoints.Path[PathIndex];
        UpdateSprite();
    }

    void UpdateSprite()
    {
        Vector3 dir = target.position - transform.position;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            spriteRenderer.sprite = dir.x > 0 ? data.moveRight : data.moveLeft;
        else
            spriteRenderer.sprite = dir.y > 0 ? data.moveUp : data.moveDown;
    }

    public float GetDistanceToWaypoint()
    {
        return Vector3.Distance(target.position, transform.position);
    }

    public bool GetHit(int damage)
    {
        if (currentHealth <= damage)
        {
            Economy.Instance.Earn(data.reward);
            WaveManager.Instance.OnEnemyDied(this);
            return true;
        }
        currentHealth -= damage;
        return false;
    }


    public void ApplySlow(float amount, float duration)
    {
        if (data.isImmuneToSlow) return;

        currentSpeed = data.speed * (1f - amount);
        slowTimer = duration;
        isSlowed = true;
    }
}