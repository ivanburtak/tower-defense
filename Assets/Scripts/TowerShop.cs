using UnityEngine;
using UnityEngine.UI;

public class TowerShop : MonoBehaviour
{
    public static TowerShop Instance { get; private set; }

    public GameObject towerPrefab;

    [SerializeField] private GameObject panel;

    [SerializeField] private TowerData archerData;
    [SerializeField] private TowerData mageData;
    [SerializeField] private TowerData freezerData;
    [SerializeField] private TowerData cannonData;

    [SerializeField] private Button archerButton;
    [SerializeField] private Button mageButton;
    [SerializeField] private Button freezerButton;
    [SerializeField] private Button cannonButton;

    [SerializeField] private TMPro.TextMeshProUGUI archerPriceText;
    [SerializeField] private TMPro.TextMeshProUGUI magePriceText;
    [SerializeField] private TMPro.TextMeshProUGUI freezerPriceText;
    [SerializeField] private TMPro.TextMeshProUGUI cannonPriceText;


    private Tile selectedTile;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    void Start()
    {
        archerPriceText.text = archerData.cost + "g";
        magePriceText.text = mageData.cost + "g";
        freezerPriceText.text = freezerData.cost + "g";
        cannonPriceText.text = cannonData.cost + "g";
    }

    public void OpenShop(Tile tile)
    {
        selectedTile = tile;
        panel.SetActive(true);
        RefreshButtons();
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

    public void RefreshButtons()
    {
        archerButton.interactable = Economy.Instance.Gold >= archerData.cost;
        mageButton.interactable = Economy.Instance.Gold >= mageData.cost;
        freezerButton.interactable = Economy.Instance.Gold >= freezerData.cost;
        cannonButton.interactable = Economy.Instance.Gold >= cannonData.cost;
    }
}