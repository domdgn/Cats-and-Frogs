using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public delegate void BalanceUpdatedEvent(int balance);
    public event BalanceUpdatedEvent OnBalanceUpdated;
    [SerializeField] private int coinCount;
    private GameUIMgr GameUIMgr;

    private void OnEnable()
    {
        OnBalanceUpdated?.Invoke(coinCount);
    }
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
        OnBalanceUpdated?.Invoke(coinCount);
    }
    public void SetMoney(int newCoinCount)
    {
        coinCount = newCoinCount;
        OnBalanceUpdated?.Invoke(coinCount);
        SendCointCountToUI();
    }

    private void SendCointCountToUI()
    {
        GameUIMgr.UpdateCoinCount(coinCount);
    }

    public void SpendMoney(int moneySpent)
    {
        coinCount -= moneySpent;
        OnBalanceUpdated?.Invoke(coinCount);
        SendCointCountToUI();
    }

    public int GetCoinCount() { return coinCount; }

    public void UpdateCurrency()
    {
        OnBalanceUpdated?.Invoke(coinCount);
    }
}
