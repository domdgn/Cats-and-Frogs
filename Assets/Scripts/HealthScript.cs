using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float health;

    public void TakeDamage(float damage)
    {
        health -= damage;
        AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.hurt);
    }

    public void AddHealth(float healthToAdd)
    {
        health += healthToAdd;
    }

    public float GetHealth()
    {
        return health;
    }
}
