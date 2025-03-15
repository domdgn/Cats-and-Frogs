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
    [Header("Cat Info")]
    public string catName;
    public float delay;
    public float health = 100;
    public float waitTime = 1f;

    public Sprite idleSprite;
    public Sprite altSprite;
    public RuntimeAnimatorController animatorController;

    [Header("Bullet Settings")]
    public float damage = 10;
    public float bulletSpeed = 5f;
    public bool burstFire = false;
    public int maxBurstShots = 3;
    public float burstWait = 1f;

    [Header("Cat Type")]
    public CatType catMode;
}
