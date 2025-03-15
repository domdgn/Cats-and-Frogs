using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIMgr : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buyTimerText, buyTimerShopText;
    [SerializeField] private TextMeshProUGUI coinCountText, coinCountShopText;
    [SerializeField] private TextMeshProUGUI onScreenScore, failScreenScore;
    private string newText;

    private ScoreManager scoreMgr;

    private void Awake()
    {
        scoreMgr = FindObjectOfType<ScoreManager>();

        PondScript pondScript = FindObjectOfType<PondScript>();
        if (pondScript != null)
        {
            pondScript.OnGameOver += GameFailed;
        }
    }
    public void UpdateTimer(float time)
    {
        time = Mathf.Round(time);
        if (buyTimerText != null)
        {
            newText = (time.ToString() + "s");
            buyTimerText.text = newText;
            buyTimerShopText.text = newText;
        }
    }

    public void UpdateCoinCount(int  coinCount)
    {
        if (buyTimerText != null)
        {
            newText = (coinCount.ToString());
            coinCountText.text = newText;
            coinCountShopText.text = newText;
        }
    }

    public void UpdateScoreCount()
    {
        if (onScreenScore != null)
        {
            newText = (scoreMgr.GetScore().ToString());
            onScreenScore.text = newText;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }

    public void GameFailed()
    {
        newText = (scoreMgr.GetScore().ToString());
        failScreenScore.text = newText;
        Time.timeScale = 0.25f;
    }
}
