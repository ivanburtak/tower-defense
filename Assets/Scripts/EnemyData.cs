using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "TD/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int cost;       // attacker budget cost
    public int reward;     // gold on kill
    public int health;
    public float speed;
    public int damage;
    public bool isImmuneToSlow;
    public Sprite moveRight;
    public Sprite moveLeft;
    public Sprite moveUp;
    public Sprite moveDown;
}