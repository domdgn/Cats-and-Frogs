using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Game/Enemy")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public Sprite sprite;
    public int health = 100;
    public float moveSpeed = 1f;
    public int damage = 10;
}