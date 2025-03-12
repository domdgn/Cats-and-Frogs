using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    private CatSO catData;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private int health;
    private float damage;
    private float waitTime;
    private float timer = 0f;

    private bool isWaiting = true;
    private bool isAttacking = false;
    private Vector2 currentGridPosition;

    private void Awake()
    {
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

        if (spriteRenderer && catType.sprite)
        {
            spriteRenderer.sprite = catType.sprite;
        }
        if (animator && catType.animatorController)
        {
            animator.runtimeAnimatorController = catType.animatorController;
        }
    }

    private void Update()
    {
        if (catData == null) return;

        timer += Time.deltaTime;
    }

    private void Attack(float damage)
    {

    }
}
