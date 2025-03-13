using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    public CatController FindCatAt(Vector2 position, float radius = 0.5f)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == gameObject) continue;

            CatController cat = collider.GetComponent<CatController>();
            if (cat != null)
            {
                return cat;
            }
        }

        return null;
    }
    public void DoMeleeDamage(GameObject catToHurt, float damage)
    {
        if (catToHurt == null) return;

        CatController cat = catToHurt.GetComponent<CatController>();
        if (cat != null)
        {
            cat.TakeDamage(damage);
        }
    }
}
