using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    private CatSO catData;
    private ProjectileFire projectileScript;
    private HealthScript healthScript;
    private CatAnimationController animationController;
    private BinCatScript binCatScript;

    private float damage;
    private float waitTime;
    private float speed;
    private float delay;
    private Vector2 currentGridPosition;

    private void Awake()
    {
        projectileScript = GetComponent<ProjectileFire>();
        healthScript = GetComponent<HealthScript>();
        currentGridPosition = transform.position;
        animationController = GetComponent<CatAnimationController>();
        binCatScript = GetComponent<BinCatScript>();
    }

    public void SetupCat(CatSO catType)
    {
        healthScript.health = catType.health;
        waitTime = catType.waitTime;
        damage = catType.damage;
        speed = catType.bulletSpeed;
        delay = catType.delay;

        animationController.Initialise(catType.idleSprite, catType.altSprite, catType.animatorController);
        if (catType.catMode == CatType.CoinCollector)
        {
            binCatScript.enabled = true;
            binCatScript.InitialiseBinCat(waitTime);
        }
        else
        {
            binCatScript.enabled = false;
        }
        StartCoroutine(AttackAndWait(delay, waitTime));
    }

    private void Attack(float damage, float speed)
    {
        if (projectileScript && projectileScript.enabled)
        {
            animationController.PlayDefaultAnimation();
            projectileScript.Fire(damage, speed);
        }
    }

    IEnumerator AttackAndWait(float delay, float waitTime)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            Attack(damage, speed);

            yield return new WaitForSeconds(waitTime);
        }
    }

    public void TakeDamage(float amount)
    {
        healthScript.TakeDamage(amount);

        StartCoroutine(animationController.HurtAnim());
        if (healthScript.GetHealth() <= 0)
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        ContainerHandler.ClearPosition(currentGridPosition);
        Destroy(gameObject);
    }
}