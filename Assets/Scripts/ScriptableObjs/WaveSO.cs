using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Game/Wave")]
public class WaveSO : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawn
    {
        public EnemySO enemyType;
        public int laneIndex;
        public float spawnDelay;
    }

    public string waveName = "Wave 1";
    public EnemySpawn[] enemies;
    public float waveDuration = 30f;
}