using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemySO enemyData;
    private SpriteRenderer spriteRenderer;
    private int health;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetupEnemy(EnemySO enemyType)
    {
        enemyData = enemyType;
        health = enemyType.health;

        if (spriteRenderer != null && enemyType.sprite != null)
        {
            spriteRenderer.sprite = enemyType.sprite;
        }
    }

    void Update()
    {
        if (enemyData != null)
        {
            transform.Translate(Vector3.down * enemyData.moveSpeed * Time.deltaTime);
            //obviously not how they will move, just a test.
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}