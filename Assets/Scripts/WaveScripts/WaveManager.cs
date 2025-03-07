using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveSO[] waves;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;

    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(StartWave(currentWave));
    }

    IEnumerator StartWave(int waveIndex)
    {
        Debug.Log("Starting wave: " + waves[waveIndex].waveName);

        foreach (var enemySpawn in waves[waveIndex].enemies)
        {
            yield return new WaitForSeconds(enemySpawn.spawnDelay);
            int lane = Mathf.Clamp(enemySpawn.laneIndex, 0, spawnPoints.Length - 1);
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[lane].position, Quaternion.identity);

            EnemyController controller = enemy.GetComponent<EnemyController>();
            if (controller != null)
            {
                controller.SetupEnemy(enemySpawn.enemyType);
            }
        }

        yield return new WaitForSeconds(waves[waveIndex].waveDuration);

        currentWave++;
        if (currentWave < waves.Length)
        {
            StartCoroutine(StartWave(currentWave));
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }
}