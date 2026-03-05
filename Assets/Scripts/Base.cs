using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int health = 20;
    public Transform waypoints;
    public Transform[] path
    {
        get;
        private set;
    }
    public TextMeshProUGUI healthText;

    void Start()
    {
        healthText.text = health.ToString();

        path = new Transform[waypoints.childCount + 1];
        for (int i = 0; i < waypoints.childCount; i++)
        {
            path[i] = waypoints.GetChild(i);
        }
        path[^1] = transform;
    }

    public bool GetHit(int damage)
    {
        if (health <= damage)
        {
            health = 0;
            healthText.text = "0";
            return true;
        }

        health -= damage;
        healthText.text = health.ToString();

        return false;
    }
}
