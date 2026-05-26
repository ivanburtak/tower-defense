using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    private Tower tower;
    private SpriteRenderer render;
    [SerializeField] private Color hoverColour;

    private Color normalColour;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();

        normalColour = render.color;
    }

    public void SetHover(bool state)
    {
        render.color = state ? hoverColour : normalColour;
    }

    public bool IsOccupied => tower != null;

    public bool PlaceTower(TowerData data)
    {
        if (tower != null) return false;
        if (!Economy.Instance.Spend(data.cost)) return false;

        GameObject obj = Instantiate(TowerShop.Instance.towerPrefab, transform.position, Quaternion.identity);
        tower = obj.GetComponent<Tower>();
        tower.data = data;
        tower.GetComponent<SpriteRenderer>().sprite = data.towerSprite;
        return true;
    }
}