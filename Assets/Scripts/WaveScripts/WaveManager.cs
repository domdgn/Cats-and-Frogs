using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveSO[] waves;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public Transform frogParent;
    private GameUIMgr gameUIMgr;
    private CameraController cameraController;
    [SerializeField] private CanvasGroup gameUI;

    [SerializeField] float buyTimer;
    private int currentWave = 0;
    private int frogsAlive = 0;
    private int totalFrogsInWave = 0;
    private Coroutine currentWaveCoroutine;
    private float remainingBuyTime;
    private void Awake()
    {
        gameUIMgr = FindObjectOfType<GameUIMgr>();
        cameraController = FindObjectOfType<CameraController>();
    }

    public void BeginGame()
    {
        BeginBuyPeriod(buyTimer);
    }
    public void BeginNextWave()
    {
        if (currentWave < waves.Length)
        {
            if (currentWaveCoroutine != null)
            {
                StopCoroutine(currentWaveCoroutine);
            }

            currentWaveCoroutine = StartCoroutine(StartWave(currentWave));
            currentWave++;
        }
    }
    IEnumerator StartWave(int waveIndex)
    {
        if (cameraController.isAtShop())
        {
            cameraController.MoveCamera();
        }

        frogsAlive = 0;
        totalFrogsInWave = waves[waveIndex].enemies.Length;

        float waveStartTime = Time.time;

        foreach (var enemySpawn in waves[waveIndex].enemies)
        {
            float spawnTime = waveStartTime + enemySpawn.spawnDelay;
            while (Time.time < spawnTime)
            {
                yield return null;
            }

            int lane = Mathf.Clamp(enemySpawn.laneIndex, 0, spawnPoints.Length - 1);
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[lane].position, Quaternion.identity, frogParent);
            frogsAlive++;

            EnemyController controller = enemy.GetComponent<EnemyController>();
            if (controller != null)
            {
                controller.SetupEnemy(enemySpawn.enemyType);
                controller.OnDeath += FrogKilled; // Subscribes frog to death event
            }
        }

        // After all enemies are spawned, wait until all are killed before proceeding
        float timeElapsed = 0f;
        while (frogsAlive > 0)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }


    public void FrogKilled()
    {
        frogsAlive--;

        if (frogsAlive <= 0)
        {
            if (currentWaveCoroutine != null)
            {
                StopCoroutine(currentWaveCoroutine);
                currentWaveCoroutine = null;
            }

            BeginBuyPeriod(buyTimer);
        }
    }
    public void BeginBuyPeriod(float buyTimer)
    {
        StartCoroutine(BuyPeriod(buyTimer));
    }
    IEnumerator BuyPeriod(float buyTimer)
    {
        gameUI.interactable = true;
        remainingBuyTime = buyTimer;

        while (remainingBuyTime > 0)
        {
            gameUIMgr.UpdateTimer(remainingBuyTime);
            remainingBuyTime -= Time.deltaTime;
            yield return null;
        }

        gameUIMgr.UpdateTimer(0f);
        gameUI.interactable = false;
        BeginNextWave();
    }
    public int GetNumberFrogs()
    {
        return frogsAlive;
    }
    public int GetTotalWaves()
    {
        return waves.Length;
    }
    public int GetCurrentWaveIndex()
    {
        return currentWave;
    }
}