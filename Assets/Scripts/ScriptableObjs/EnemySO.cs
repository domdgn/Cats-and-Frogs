using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFrog", menuName = "Game/Frog")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public Sprite sprite;
    public int health = 100;
    public float moveSpeed = 1f;
    public float waitTime = 2.5f;
    public int damage = 10;
    public AnimatorController animatorController;
}