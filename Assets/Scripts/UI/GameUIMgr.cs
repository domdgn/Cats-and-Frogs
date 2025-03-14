using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIMgr : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buyTimerText, buyTimerShopText;
    [SerializeField] private TextMeshProUGUI coinCountText, coinCountShopText;
    private string newText;
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
}
