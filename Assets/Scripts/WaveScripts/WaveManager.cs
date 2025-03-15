using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveSO[] waves;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public Transform frogParent;
    private CoinSpawner coinSpawner;
    private GameUIMgr gameUIMgr;
    private CameraController cameraController;
    [SerializeField] private CanvasGroup gameUI;
    [SerializeField] private GameObject warningSpritePrefab;
    private List<GameObject> warningSprites = new List<GameObject>();

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
        coinSpawner = FindObjectOfType<CoinSpawner>();
    }

    public void BeginGame()
    {
        BeginBuyPeriod(buyTimer);
        coinSpawner.BeginCoinSpawner();
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
        AudioPlayer.Instance.SwitchMusic(AudioPlayer.Instance.atkMusic);

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

    private void SpawnWarningSprites(int waveIndex)
    {
        // Clear any existing warning sprites
        ClearWarningSprites();

        foreach (var enemySpawn in waves[waveIndex].enemies)
        {
            int lane = Mathf.Clamp(enemySpawn.laneIndex, 0, spawnPoints.Length - 1);
            GameObject warning = Instantiate(warningSpritePrefab, spawnPoints[lane].position, Quaternion.identity, frogParent);
            warningSprites.Add(warning);
        }
    }

    private void ClearWarningSprites()
    {
        foreach (var sprite in warningSprites)
        {
            if (sprite != null)
            {
                Destroy(sprite);
            }
        }
        warningSprites.Clear();
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

        // Spawn warning sprites for the next wave if there is one
        if (currentWave < waves.Length)
        {
            SpawnWarningSprites(currentWave);
        }
    }

    IEnumerator BuyPeriod(float buyTimer)
    {
        //AudioPlayer.Instance.SwitchMusic(AudioPlayer.Instance.waitMusic);
        gameUI.interactable = true;
        remainingBuyTime = buyTimer;

        HashSet<int> playedSeconds = new HashSet<int>();

        while (remainingBuyTime > 0)
        {
            int currentSecond = Mathf.FloorToInt(remainingBuyTime);

            gameUIMgr.UpdateTimer(currentSecond);

            if (currentSecond <= 5 && currentSecond >= 1 && !playedSeconds.Contains(currentSecond))
            {
                AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.blip);
                playedSeconds.Add(currentSecond);
            }

            remainingBuyTime -= Time.deltaTime;
            yield return null;
        }

        gameUIMgr.UpdateTimer(0);
        gameUI.interactable = false;

        ClearWarningSprites();

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