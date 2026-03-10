using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int health = 3;

    private Transform target;
    public int pathIndex
    {
        get;
        private set;
    } = 0;

    void Start()
    {
        target = Waypoints.path[0];
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (transform.position == target.position)
            NextWaypoint();
    }

    void NextWaypoint()
    {
        pathIndex++;
        if (pathIndex == Waypoints.path.Length)
        {
            Base.Instance.GetHit(damage);
            Spawner.Instance.DestroyEnemy(this);
            return;
        }

        target = Waypoints.path[pathIndex];
    }

    public float GetDistanceToWaypoint()
    {
        return Vector3.Distance(target.position, transform.position);
    }

    public bool GetHit(int damage)
    {
        if (health <= damage)
        {
            Spawner.Instance.DestroyEnemy(this);
            return true;
        }

        health -= damage;

        return false;
    }
}
