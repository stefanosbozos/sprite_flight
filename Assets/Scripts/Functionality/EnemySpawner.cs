using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyWaveSO[] EnemyWave;
    public float SpawingAreaOffsetX = 1f;
    public float SpawningAreaOffsetY = 3f;
    public GameObject SpawnEnemyVFX;

    private Transform m_player;

    private Camera m_Camera;
    private float m_cameraSizeX;
    private float m_cameraSizeY;

    private float m_spawningPositionLimitX;
    private float m_spawningPositionLimitY;

    private int m_enemyToSpawnIndex;
    private int m_waveIndex;
    private int m_totalEnemiesSpawned;
    private GameObject[] m_enemiesOnScreen;

    private float m_timer = 0f;
    private bool m_prepareNextWave;



    void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Start()
    {
        m_cameraSizeX = m_Camera.orthographicSize * 16 / 9;
        m_cameraSizeY = m_Camera.orthographicSize;

        // Decide the x and y of the spawning position based on the Camera's orthographic size
        m_spawningPositionLimitX = m_cameraSizeX - SpawingAreaOffsetX;
        m_spawningPositionLimitY = m_cameraSizeY - SpawningAreaOffsetY;
    }

    void Update()
    {

        if (!AllWavesCleared())
        {

            if (EnemiesLeftInWave())
            {
                SpawnEnemy();
            }
            else
            {
                PrepareNextWave();
            }

        }


        if (!PlayerIsAlive())
        {
            EnemyRuntimeSetSO.KillAll();
        }

    }

    private void SpawnEnemy()
    {
        m_timer += Time.deltaTime;

        if (IsReadyToSpawn())
        {
            Vector3 randomSpawnPosition = GetRandomSpawnPosition();
            if (PlayerIsAlive())
            {
                if (randomSpawnPosition != m_player.transform.position)
                {
                    PlaySpawnVFX(randomSpawnPosition);
                    Instantiate(GetRandomEnemyType(), randomSpawnPosition, Quaternion.identity);
                    Debug.Log(EnemyRuntimeSetSO.Size());
                }
            }
            
            m_totalEnemiesSpawned++;
            m_timer = 0f;
        }

    }

    private void PrepareNextWave()
    {
        if (EnemyRuntimeSetSO.Size() <= 0 && !m_prepareNextWave)
        {
            m_prepareNextWave = true;
            StartCoroutine(SpawnNextWave());
        }
    }

    private IEnumerator SpawnNextWave()
    {
        Debug.Log("Preparing Wave...");

        yield return new WaitForSeconds(EnemyWaveSO.m_TimeBetweenEachWave);
        m_totalEnemiesSpawned = 0;
        m_waveIndex++;
        m_prepareNextWave = false;

        Debug.Log("Next Wave is ready.");
    }

    private bool IsReadyToSpawn()
    {
        bool spawnIntervalComplete = m_timer >= EnemyWave[m_waveIndex].EnemySpawningInterval;
        bool doesNotExceedEnemiesOnScreen = EnemyRuntimeSetSO.Size() < EnemyWave[m_waveIndex].EnemiesOnScreen;

        return spawnIntervalComplete && doesNotExceedEnemiesOnScreen;
    }

    private GameObject GetRandomEnemyType()
    {
        m_enemyToSpawnIndex = Random.Range(0, EnemyWave[m_waveIndex].EnemyType.Length);
        return EnemyWave[m_waveIndex].EnemyType[m_enemyToSpawnIndex];
    }

    private bool EnemiesLeftInWave()
    {
        return EnemyWave[m_waveIndex].EnemiesPerWave > m_totalEnemiesSpawned;
    }

    Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-m_spawningPositionLimitX, m_spawningPositionLimitX + 1);
        float randomY = Random.Range(-m_spawningPositionLimitY, m_spawningPositionLimitY + 1);

        return new Vector3(randomX, randomY, 0);
    }

    private void PlaySpawnVFX(Vector3 position)
    {
        GameObject spawnEffect = Instantiate(SpawnEnemyVFX, position, Quaternion.identity);
        Destroy(spawnEffect, 1f);
    }
    
    private bool PlayerIsAlive()
    {
        return m_player != null;
    }

    private bool AllWavesCleared()
    {
        return m_waveIndex >= EnemyWave.Length;
    }

}
