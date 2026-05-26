using UnityEngine;

public class TowerShop : MonoBehaviour
{
    public static TowerShop Instance { get; private set; }
    public GameObject towerPrefab;

    [SerializeField] private GameObject panel;

    private Tile selectedTile;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void OpenShop(Tile tile)
    {
        selectedTile = tile;
        panel.SetActive(true);
    }

    public void CloseShop()
    {
        selectedTile = null;
        panel.SetActive(false);
    }

    public bool IsActive => panel.activeInHierarchy;

    public void SelectTower(TowerData data)
    {
        if (selectedTile == null) return;
        if (selectedTile.PlaceTower(data)) CloseShop();
    }
}