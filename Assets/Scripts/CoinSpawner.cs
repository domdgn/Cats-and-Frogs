using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float minSpawnTime = 15f;
    [SerializeField] private float maxSpawnTime = 35f;
    [SerializeField] private int maxCoins = 10;

    private Coroutine spawnRoutine;

    public void BeginCoinSpawner()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }

        spawnRoutine = StartCoroutine(SpawnCoinsRoutine());
    }

    private IEnumerator SpawnCoinsRoutine()
    {
        while (true)
        {
            if (CurrencyManager.Instance.GetCoinCount() <= maxCoins)
            {
                float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(waitTime);
                Instantiate(coinPrefab, transform.position, Quaternion.identity, transform);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void StopCoinSpawner()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }
}