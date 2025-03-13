using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    private CatSO catData;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private ProjectileFire projectileScript;

    private int health;
    private float damage;
    private float waitTime;
    private float speed;

    private float delay = 0.2f;
    private bool isAttacking = false;
    private Vector2 currentGridPosition;

    private void Awake()
    {
        projectileScript = GetComponent<ProjectileFire>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetupCat(CatSO catType)
    {
        Debug.Log(catType.name);
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


        if (projectileScript.enabled && isAttacking)
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
            isAttacking = true;
            Attack(damage, speed);
            yield return new WaitForSeconds(waitTime);
            isAttacking = false;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
