using UnityEngine;

[CreateAssetMenu(fileName = "NewFrog", menuName = "Game/Frog")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public Sprite sprite;
    public float health = 100;
    public float moveSpeed = 1f;
    public float waitTime = 2.5f;
    public float damage = 10;
    public RuntimeAnimatorController animatorController;
}