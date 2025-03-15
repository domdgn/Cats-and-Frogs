using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondScript : MonoBehaviour
{
    public float health;

    public delegate void GameOverEvent();
    public event GameOverEvent OnGameOver;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{this.gameObject.name} has {health} health");

        if ( health <= 0 )
        {
            EndGame();
        }
    }

    public void AddHealth(float healthToAdd)
    {
        health += healthToAdd;
    }

    public float GetPondHealth()
    {
        return health;
    }

    private void EndGame()
    {
        OnGameOver?.Invoke();
        Debug.Log("GAME OVER!");
    }
}
