using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    public static Base Instance { get; private set; }

    [SerializeField] private int health = 20;
    [SerializeField] private TextMeshProUGUI healthText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        healthText.text = health.ToString();
    }

    public bool GetHit(int damage)
    {
        health = Mathf.Max(0, health - damage);
        healthText.text = health.ToString();
        if (health == 0)
        {
            GameManager.Instance.OnBaseDead();
            return true;
        }
        return false;
    }
}
