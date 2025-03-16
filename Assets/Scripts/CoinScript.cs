using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private float moveTimer = 0f;
    private float moveInterval = 3f;
    private float lerpValue = 0f;
    private Camera mainCamera;

    private Vector3 originalScale;
    private float pulseAmount = 0.1f;
    private float pulseSpeed = 1.5f;
    private bool isMoving = true;
    private int coinValue;

    private void Awake()
    {
        startPos = transform.position;
        mainCamera = Camera.main;
        originalScale = transform.localScale;
        coinValue = Random.Range(-7, -3);
        SetRandomPositionOnScreen();
        StartCoroutine(waitThenDie());
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            if (touch.phase == TouchPhase.Ended)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    CurrencyManager.Instance.SpendMoney(coinValue);
                    AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.coinCollect);
                    Destroy(gameObject);
                }
            }
        }

        if (isMoving)
        {
            moveTimer += Time.deltaTime;
            lerpValue = Mathf.Clamp01(moveTimer / moveInterval);

            float smoothLerp = Mathf.SmoothStep(0, 1, lerpValue);
            transform.position = Vector3.Lerp(startPos, endPos, smoothLerp);

            if (lerpValue >= 1.0f)
            {
                isMoving = false;
            }
        }
        else
        {
            float pulseScale = 1f + pulseAmount * Mathf.Sin(Time.time * pulseSpeed);
            transform.localScale = originalScale * pulseScale;
        }
    }

    private void SetRandomPositionOnScreen()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        Vector3 randomScreenPoint = new Vector3(
            Random.Range(screenWidth * 0.1f, screenWidth * 0.9f),
            Random.Range(screenHeight * 0.1f, screenHeight * 0.9f),
            0
        );

        Vector3 randomWorldPoint = mainCamera.ScreenToWorldPoint(randomScreenPoint);
        randomWorldPoint.z = 0;

        endPos = randomWorldPoint;
    }

    IEnumerator waitThenDie()
    {
        yield return new WaitForSeconds(7.5f);
        Destroy(gameObject);
    }
}