using UnityEngine;

[CreateAssetMenu(fileName = "NewCat", menuName = "Game/Cat")]
public class CatSO : ScriptableObject
{
    public string catName;
    public Sprite sprite;
    public int health = 100;
    public float attackSpeed = 1f;
    public int damage = 10;
}