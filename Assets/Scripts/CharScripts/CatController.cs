using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    private CatSO catData;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private ProjectileFire projectileScript;

    private float health;
    private float damage;
    private float waitTime;
    private float speed;

    private float delay = 0.2f;
    private Vector2 currentGridPosition;

    private void Awake()
    {
        projectileScript = GetComponent<ProjectileFire>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentGridPosition = transform.position;
    }

    public void SetupCat(CatSO catType)
    {
        //Debug.Log(catType.name);
        catData = catType;
        health = catType.health;
        waitTime = catType.waitTime;
        damage = catType.damage;
        speed = catType.bulletSpeed;

        if (spriteRenderer && catType.sprite)
        {
            spriteRenderer.sprite = catType.sprite;
        }
        if (animator && catType.animatorController)
        {
            animator.runtimeAnimatorController = catType.animatorController;
        }

        StartCoroutine(AttackAndWait(delay, waitTime));
    }

    private void Attack(float damage, float speed)
    {

        //MAKE THIS SYNC WITH ANIMATIONS AT SOME POINT
        //animation events


        if (projectileScript.enabled)
        {
            projectileScript.Fire(damage, speed);
        }
        else return;
    }

    IEnumerator AttackAndWait(float delay, float waitTime)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            Attack(damage, speed);
            yield return new WaitForSeconds(waitTime);

            yield return new WaitForSeconds(0.1f);
        }
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            DestroySelf();
        }
    }
    private void DestroySelf()
    {
        Debug.Log("destyroyded cat");
        ContainerHandler.ClearPosition(currentGridPosition);
        Destroy(gameObject);
    }
}
