using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public int health = 20;
    private Base destination;
    private Transform target;
    private int pathIndex = 0;

    public void Initialise(Base destination)
    {
        this.destination = destination;
        target = destination.path[0];
    }


    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (transform.position == target.position)
        {
            NextWaypoint();
        }
    }

    void NextWaypoint()
    {
        pathIndex++;
        if (pathIndex == destination.path.Length)
        {
            destination.GetHit(damage);
            Destroy(gameObject);
            return;
        }

        target = destination.path[pathIndex];
    }

    public bool GetHit(int damage)
    {
        if (health <= damage)
        {
            Destroy(gameObject);
            return true;
        }

        health -= damage;

        return false;
    }
}
