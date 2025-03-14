using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform firePoint;
    private ProjectileCollision projCollisionScript;
    public void Fire(float damage, float speed)
    {
        GameObject projectile = Instantiate(prefab, firePoint.position, Quaternion.identity);
        projCollisionScript = projectile.GetComponent<ProjectileCollision>();

        projCollisionScript.SetDamage(damage);

        ProjectileMovement movement = projectile.GetComponent<ProjectileMovement>();
        if (movement != null)
        {
            movement.SetupProjectile(damage, speed);
        }
    }
}
