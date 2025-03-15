using System.Collections;
using UnityEngine;

public class BinCatScript : MonoBehaviour
{
    private Vector3 touchPosition;
    private CatAnimationController animController;
    private bool hasCoin = false;
    private bool isCoinRoutineRunning = false;
    private Sprite alt;
    [SerializeField] private GameObject coinSpriteHeld;

    private void Awake()
    {
        animController = GetComponent<CatAnimationController>();
        coinSpriteHeld.SetActive(false);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            if (touch.phase == TouchPhase.Ended)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject && hasCoin)
                {
                    CurrencyManager.Instance.SpendMoney(-5);
                    AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.coinCollect);
                    hasCoin = false;
                    coinSpriteHeld.SetActive(false);
                    animController.PlayDefaultAnimation();
                    //animController.SetSpriteColor(Color.white);

                    if (!isCoinRoutineRunning)
                    {
                        StartCoroutine(CoinSpawnRoutine());
                    }
                }
            }
        }
    }

    public void InitialiseBinCat(float timeToW)
    {
        StartCoroutine(StartBin(timeToW));
    }

    IEnumerator StartBin(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        if (!isCoinRoutineRunning)
        {
            StartCoroutine(CoinSpawnRoutine());
        }
    }

    IEnumerator CoinSpawnRoutine()
    {
        isCoinRoutineRunning = true;
        float waitTime = Random.Range(5f, 10f);
        yield return new WaitForSeconds(waitTime);
        hasCoin = true;
        animController.PlayWaitAnimation();
        yield return new WaitForSeconds(1);
        coinSpriteHeld.SetActive(true);
        //animController.SetSpriteColor(Color.yellow);
        isCoinRoutineRunning = false;
    }
}