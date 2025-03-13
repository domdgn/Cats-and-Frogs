using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    static float damage;
    private EnemyController enemyController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Frog"))
        {
            //Debug.Log($"Hit Frog for {damage}");
            enemyController = collision.gameObject.GetComponent<EnemyController>();
            enemyController.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}
