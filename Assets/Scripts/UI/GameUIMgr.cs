using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIMgr : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buyTimerText;
    [SerializeField] private TextMeshProUGUI coinCountText;
    private string newText;
    public void UpdateTimer(float time)
    {
        time = Mathf.Round(time);
        if (buyTimerText != null)
        {
            newText = (time.ToString() + "s");
            buyTimerText.text = newText;
            Debug.Log("timer text updated to" + newText);
        }
    }
}
