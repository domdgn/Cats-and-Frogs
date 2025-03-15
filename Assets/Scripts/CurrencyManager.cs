using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    [SerializeField] private int coinCount;
    private GameUIMgr GameUIMgr;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameUIMgr = FindObjectOfType<GameUIMgr>();
        SendCointCountToUI();
    }
    public void SetMoney(int newCoinCount)
    {
        coinCount = newCoinCount;
        SendCointCountToUI();
    }

    private void SendCointCountToUI()
    {
        GameUIMgr.UpdateCoinCount(coinCount);
    }

    public void SpendMoney(int moneySpent)
    {
        coinCount -= moneySpent;
        SendCointCountToUI();
    }

    public int GetCoinCount() { return coinCount; }
}
