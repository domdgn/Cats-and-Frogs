using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HealthBoostBuy : MonoBehaviour
{
    private PondScript pondScript;
    [SerializeField] private float healthBoostAmt;
    [SerializeField] private int cost;
    private void Awake()
    {
        pondScript = FindObjectOfType<PondScript>();
    }


    // NO MATTER WHAT I DID, I CANT GET THIS TO WORK WITH TOUCHMANAGER... FML
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;

                Collider2D collider = GetComponent<Collider2D>();
                if (collider != null && collider.OverlapPoint(touchPosition))
                {
                    Debug.Log("Health boost direct touch detected!");
                    if (CurrencyManager.Instance.GetCoinCount() < cost) return;
                    CurrencyManager.Instance.SpendMoney(cost);
                    pondScript.AddHealth(healthBoostAmt);
                    AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.select);
                }
            }
        }
    }
}
