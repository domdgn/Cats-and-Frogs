using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondScript : MonoBehaviour
{
    public float health;

    public delegate void GameOverEvent();
    public event GameOverEvent OnGameOver;
    public delegate void UpdatePondHealthEvent(float health);
    public event UpdatePondHealthEvent UpdatePondUI;

    private void Awake()
    {
        UpdateUI();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.pondHurt);

        UpdateUI();

        if ( health <= 0 )
        {
            EndGame();
            AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.gameOver);
        }
    }

    public void AddHealth(float healthToAdd)
    {
        health += healthToAdd;
        UpdateUI();
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

    private void UpdateUI()
    {
        UpdatePondUI?.Invoke(health);
    }
}
