using UnityEngine;

public enum CatType
{
    Gun,
    Melee,
    CoinCollector
}

[CreateAssetMenu(fileName = "NewCat", menuName = "Game/Cat")]
public class CatSO : ScriptableObject
{
    public string catName;
    public Sprite idleSprite;
    public float delay;
    public float health = 100;
    public float waitTime = 1f;
    public RuntimeAnimatorController animatorController;
    public float damage = 10;
    public float bulletSpeed = 5f;

    public CatType catMode;
}