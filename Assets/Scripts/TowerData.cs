using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TD/Tower Data")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public int cost;
    public float range;
    public float fireRate;
    public float projectileSpeed;
    public int damage;
    public float rotationSpeed = 180f;
    public float aoeRadius; // 0.0 = no AoE
    public float slowAmount; // 0.0 to 1.0, e.g. 0.0 = no slow 0.5 = half speed

    public GameObject prefab;
    public GameObject projectilePrefab;
}