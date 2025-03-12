using System.Threading;
using UnityEditor;
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
    private Vector2 currentGridPosition;
    private int distToEnd = 9;
    //private bool atEnd = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentGridPosition = new Vector2(transform.position.x,transform.position.y);
        ContainerHandler.OccupyPosition(currentGridPosition, gameObject);
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

                nextPosition = new Vector2(transform.position.x, transform.position.y - moveDistance); 

                if (!ContainerHandler.IsPositionOccupied(nextPosition))
                {
                    isWaiting = false;
                    nextPosition = new Vector3(nextPosition.x, nextPosition.y, transform.position.z);

                    ContainerHandler.ClearPosition(currentGridPosition);
                }
            }
        }
        else
        {
            float step = enemyData.moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, step);
            ContainerHandler.OccupyPosition(nextPosition, gameObject);

            if (Vector3.Distance(transform.position, nextPosition) < 0.01f)
            {
                transform.position = nextPosition;
                isWaiting = true;
                timer = 0f;
                distToEnd -= 1;

                if (distToEnd == 0)
                {
                    AtEnd();
                }

                currentGridPosition = new Vector2(transform.position.x, transform.position.y);
            }
        }
    }

    private void AtEnd()
    {
        //atEnd = true;

        Debug.Log("Frog Reached End");
        DestroySelf();

        //this is genuinely such bad code im embarrassed
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        ContainerHandler.ClearPosition(currentGridPosition);
        ContainerHandler.ClearPosition(nextPosition);
        Destroy(gameObject);
    }
}