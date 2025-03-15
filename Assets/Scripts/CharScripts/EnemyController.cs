using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemySO enemyData;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private EnemyAttackScript attackScript;
    private HealthScript healthScript;
    private PondScript pondScript;
    private CatAnimationController animController;
    private ScoreManager scoreMgr;

    private float health;
    private float waitTime;
    private float moveDistance = 1.125f;
    private float timer = 0f;
    private float damage;
    private float speed;

    private bool isWaiting = true;
    private Vector3 nextPosition;
    private Vector2 currentGridPosition;
    private int distToEnd = 9;
    private float delay;
    private int scoreToAdd;
    private GameObject catToHurt;

    public delegate void DeathEvent();
    public event DeathEvent OnDeath;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        attackScript = GetComponent<EnemyAttackScript>();
        healthScript = GetComponent<HealthScript>();
        pondScript = FindObjectOfType<PondScript>();
        animController = GetComponent<CatAnimationController>();
        scoreMgr = FindObjectOfType<ScoreManager>();
    }

    private void Start()
    {
        currentGridPosition = new Vector2(transform.position.x,transform.position.y);
        ContainerHandler.OccupyPosition(currentGridPosition, gameObject);
    }

    public void SetupEnemy(EnemySO enemyType)
    {
        damage = enemyType.damage;
        enemyData = enemyType;
        healthScript.health = enemyType.health;
        waitTime = enemyType.waitTime;
        delay = 0f;
        scoreToAdd = enemyType.scoreUponDeath;

        if (spriteRenderer != null && enemyType.sprite != null)
        {
            spriteRenderer.sprite = enemyType.sprite;
        }
        if (animator != null && enemyType.animatorController != null)
        {
            animator.runtimeAnimatorController = enemyType.animatorController;
        }

        StartCoroutine(AttackAndWait(delay, waitTime));
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
        pondScript.TakeDamage(damage);
        OnDeath?.Invoke();
        DestroySelf();
        //this is genuinely such bad code im embarrassed
    }

    public void TakeDamage(float amount)
    {
        StartCoroutine(animController.HurtAnim());
        healthScript.TakeDamage(amount);
        if (healthScript.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeath?.Invoke();
        scoreMgr.IncreaseScoreBy(scoreToAdd);
        AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.frogDeath);
        DestroySelf();
    }

    private void DestroySelf()
    {
        ContainerHandler.ClearPosition(currentGridPosition);
        ContainerHandler.ClearPosition(nextPosition);
        Destroy(gameObject);
    }
    private void MeleeAttack(Vector3 position, float damage)
    {
        if (attackScript == null)
        {
            Debug.LogError("AttackScript is null on " + gameObject.name);
            return;
        }
        Vector2 attackPosition = new Vector2(transform.position.x, transform.position.y - moveDistance);

        CatController cat = attackScript.FindCatAt(attackPosition);

        if (cat != null)
        {
            attackScript.DoMeleeDamage(cat.gameObject, damage);
        }
    }

    IEnumerator AttackAndWait(float delay, float waitTime)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            MeleeAttack(nextPosition, damage);
            yield return new WaitForSeconds(waitTime);

            yield return new WaitForSeconds(0.1f);
        }
    }
}