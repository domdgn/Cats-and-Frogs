using System.Collections;
using UnityEngine;

public class BinCatScript : MonoBehaviour
{
    private CatAnimationController animController;
    private InputManager inputManager;
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
        StartCoroutine(WaitForInputManager());
    }

    IEnumerator WaitForInputManager()
    {
        while (InputManager.Instance == null)
        {
            Debug.LogWarning("Waiting for InputManager...");
            yield return null;
        }

        inputManager = InputManager.Instance;
        inputManager.OnInputEnded += HandleInputEnded;

        Debug.Log("InputManager found, subscribed to events.");
    }

    private void OnDisable()
    {
        // Unsubscribe from input events
        if (inputManager != null)
        {
            inputManager.OnInputEnded -= HandleInputEnded;
        }

        StopAllCoroutines();
        isCoinRoutineRunning = false;
    }

    private void HandleInputEnded(Vector3 inputPosition, InputManager.InputData inputData)
    {
        if (inputManager.IsObjectTouched(gameObject, inputPosition))
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

        float waitTime = Random.Range(7.5f, 15f);
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