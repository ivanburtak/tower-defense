using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    private int waypointIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = Waypoints.points[0];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) == 0f)
        {
            NextWaypoint();
        }
    }

    void NextWaypoint()
    {
        waypointIndex++;
        if (waypointIndex == Waypoints.points.Length)
        {
            Destroy(gameObject);
            return;
        }

        target = Waypoints.points[waypointIndex];
    }
}
