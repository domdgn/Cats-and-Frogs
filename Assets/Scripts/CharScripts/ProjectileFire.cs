using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform firePoint;
    public void Fire(float damage, float speed)
    {
        Debug.Log($"Fired Projectile with {damage} damage.");
        GameObject projectile = Instantiate(prefab, firePoint.position, Quaternion.identity);

        ProjectileMovement movement = projectile.GetComponent<ProjectileMovement>();
        if (movement != null)
        {
            movement.SetupProjectile(damage, speed);
        }
    }
}
