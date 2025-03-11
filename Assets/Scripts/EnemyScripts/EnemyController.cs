using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemySO enemyData;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private int health;
    private float waitTime;
    private float moveDistance = 1.125f;
    private float timer = 0f;

    private bool isWaiting = true;
    private Vector3 nextPosition;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetupEnemy(EnemySO enemyType)
    {
        enemyData = enemyType;
        health = enemyType.health;
        waitTime = enemyType.waitTime;

        if (spriteRenderer != null && enemyType.sprite != null)
        {
            spriteRenderer.sprite = enemyType.sprite;
        }
        if (animator != null && enemyType.animatorController != null)
        {
            animator.runtimeAnimatorController = enemyType.animatorController;
        }
    }

    void Update()
    {
        if (enemyData == null) return;

        timer += Time.deltaTime;

        if (isWaiting)
        {
            if (timer >= waitTime)
            {
                timer = 0f;
                isWaiting = false;
                nextPosition = transform.position + Vector3.down * moveDistance;
            }
        }
        else
        {
            float step = enemyData.moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, step);

            if (Vector3.Distance(transform.position, nextPosition) < 0.01f)
            {
                transform.position = nextPosition;
                isWaiting = true;
                timer = 0f;
            }
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