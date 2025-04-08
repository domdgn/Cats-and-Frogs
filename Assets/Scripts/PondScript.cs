using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondScript : MonoBehaviour
{
    public float health;

    public delegate void GameOverEvent();
    public event GameOverEvent OnGameOver;
    public delegate void OnPondHurtEvent(float health);
    public event OnPondHurtEvent OnPondHurt;

    private void Awake()
    {
        OnPondHurt?.Invoke(health);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        OnPondHurt?.Invoke(health);
        AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.pondHurt);

        if ( health <= 0 )
        {
            EndGame();
            AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.gameOver);
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
