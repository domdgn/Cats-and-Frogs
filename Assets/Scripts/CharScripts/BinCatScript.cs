using System.Collections;
using UnityEngine;

public class BinCatScript : MonoBehaviour
{
    private CatAnimationController animController;
    private bool hasCoin = false;
    private bool isCoinRoutineRunning = false;
    [SerializeField] private GameObject coinSpriteHeld;

    private void Awake()
    {
        animController = GetComponent<CatAnimationController>();
        coinSpriteHeld.SetActive(false);
    }

    private void OnEnable()
    {
        // Subscribe to touch events
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.OnTouchEnded += HandleTouchEnded;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from touch events
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.OnTouchEnded -= HandleTouchEnded;
        }

        StopAllCoroutines();
        isCoinRoutineRunning = false;
    }

    private void HandleTouchEnded(Vector3 touchPosition, Touch touch)
    {
        if (TouchManager.Instance.IsObjectTouched(gameObject, touchPosition))
        {
            Debug.Log("Bin Cat Touched");

            if (!hasCoin) return;
            isCoinRoutineRunning = false;
            CurrencyManager.Instance.SpendMoney(-5);
            AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.coinCollect);
            coinSpriteHeld.SetActive(false);
            animController.PlayDefaultAnimation();
            hasCoin = false;
            StartCoroutine(CoinSpawnRoutine());
        }
    }

    public void InitialiseBinCat(float timeToW)
    {
        StartCoroutine(StartBin(timeToW));
    }

    IEnumerator StartBin(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        StartCoroutine(CoinSpawnRoutine());
    }

    IEnumerator CoinSpawnRoutine()
    {
        Debug.Log("Starting coin routine");
        isCoinRoutineRunning = true;
        float waitTime = Random.Range(5f, 10f);
        Debug.Log($"Waiting for {waitTime} seconds");
        yield return new WaitForSeconds(waitTime);
        //Debug.Log("Playing wait animation");
        animController.PlayWaitAnimation();
        yield return new WaitForSeconds(0.25f);
        Debug.Log("Coin ready");
        coinSpriteHeld.SetActive(true);
        yield return null;
        hasCoin = true;
    }
}