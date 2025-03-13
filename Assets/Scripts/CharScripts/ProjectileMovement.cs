using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float life = 3f;
    private float damage;

    private void Start()
    {
        Destroy(gameObject, life);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    public void SetupProjectile(float newDamage, float newSpeed)
    {
        damage = newDamage;
        speed = newSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if hit frgo
        // do damage
        // kill projectile
        // do in other script though (ProjectileCollision.cs)
    }
}