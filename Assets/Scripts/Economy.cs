using TMPro;
using UnityEngine;

public class Economy : MonoBehaviour
{
    public static Economy Instance { get; private set; }

    [SerializeField] private int startGold = 300;
    [SerializeField] private TextMeshProUGUI goldText;

    public int Gold { get; private set; }

    void Awake()
    {
        Instance = this;
        Gold = startGold;
        UpdateUI();
    }

    public bool Spend(int amount)
    {
        if (Gold < amount) return false;
        Gold -= amount;
        UpdateUI();
        return true;
    }

    public void Earn(int amount)
    {
        Gold += amount;
        UpdateUI();
    }

    void UpdateUI() => goldText.text = "Gold: " + Gold;
}