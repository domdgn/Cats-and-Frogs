using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int Score = 0;
    private GameUIMgr uiMgr;

    void Awake()
    {
        uiMgr = FindObjectOfType<GameUIMgr>();
    }

    public void IncreaseScoreBy(int scoreToAdd)
    {
        Score += scoreToAdd;
        uiMgr.UpdateScoreCount();
    }

    public int GetScore()
    {
        return Score;
    }
}
