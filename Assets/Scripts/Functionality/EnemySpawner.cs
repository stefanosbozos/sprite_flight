using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyWaveSO> EnemyWave;
    public float SpawingAreaOffsetX = 1f;
    public float SpawningAreaOffsetY = 3f;
    public GameObject SpawnEnemyVFX;


    // The position of the player in the world
    private Transform m_player;


    // Main Camera to get the screen size
    private Camera m_Camera;
    private float m_cameraSizeX;
    private float m_cameraSizeY;


    // The spawning position limits on screen
    private float m_spawningPositionLimitX;
    private float m_spawningPositionLimitY;


    // The index in the List of enemy types to spawn
    private int m_enemyToSpawnIndex;
    private int m_waveIndex;
    private int m_totalEnemiesSpawned;

    // The timer that updates from the deltaTime
    private float m_timer = 0f;

    private bool m_gameOver;
    private bool m_waveIsFinished;


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

        m_waveIndex = 0;
        m_gameOver = false;
    }


    void Update()
    {
        if (m_player == null)
        {
            DestroyAllEnemiesOnScreen();
            m_gameOver = !m_gameOver;
        }
        else
        {
            if (!m_waveIsFinished)
            {
                SpawnEnemy();
            }
            else
            {
                SpawnNextWave();
            }
        }
    }


    private void SpawnEnemy()
    {
        m_timer += Time.deltaTime;
        // Check the spawning interval and how many enemies are on the screen.
        if (IsReadyToSpawn() && EnemiesLeftInWave())
        {

            m_enemyToSpawnIndex = Random.Range(0, EnemyWave[m_waveIndex].EnemyType.Count);
            Vector3 position = GetRandomSpawnPosition();
            GameObject enemyTypeToSpawn = EnemyWave[m_waveIndex].EnemyType[m_enemyToSpawnIndex];

            if (position != m_player.transform.position)
            {
                GameObject spawnEffect = Instantiate(SpawnEnemyVFX, position, Quaternion.identity);
                Instantiate(enemyTypeToSpawn, position, Quaternion.identity);

                m_totalEnemiesSpawned++;

                m_timer = 0f;
                Destroy(spawnEffect, 1f);
            }
        }

        if (!EnemiesLeftInWave())
        {
            m_waveIsFinished = !m_waveIsFinished;
            Debug.Log("Wave is finished: " + m_waveIsFinished);
        }

    }



    private bool IsReadyToSpawn()
    {
        bool spawnIntervalComplete = m_timer >= EnemyWave[m_waveIndex].EnemySpawningInterval;
        bool doesNotExceedEnemiesOnScreen = CountEnemiesOnScreen() < EnemyWave[m_waveIndex].EnemiesOnScreen;

        return spawnIntervalComplete && doesNotExceedEnemiesOnScreen;
    }


    private bool EnemiesLeftInWave()
    {
        return EnemyWave[m_waveIndex].EnemiesPerWave > m_totalEnemiesSpawned;
    }

    private void SpawnNextWave()
    {
        m_timer += Time.deltaTime;

        if (m_timer > EnemyWaveSO.m_TimeBetweenEachWave)
        {
            if (m_waveIndex < EnemyWave.Count)
            {
                m_waveIndex++;
                Debug.Log("Wave Index: " + m_waveIndex);
            }
            else
            {
                m_gameOver = !m_gameOver;
                Debug.Log("All waves have been cleared! Game Over!");
            }
        }
        
    }

    private int CountEnemiesOnScreen()
    {
        GameObject[] enemiesOnScreen = new GameObject[0];

        for (int i = 0; i < EnemyWave[m_waveIndex].EnemyType.Count; i++)
        {
            string currentEnemyTag = EnemyWave[m_waveIndex].EnemyType[i].tag;
            enemiesOnScreen = GameObject.FindGameObjectsWithTag(currentEnemyTag);
        }

        return enemiesOnScreen.Length;
    }


    private void DestroyAllEnemiesOnScreen()
    {
        for (int i = 0; i < EnemyWave[m_waveIndex].EnemyType.Count; i++)
        {
            string currentEnemyTag = EnemyWave[m_waveIndex].EnemyType[i].tag;
            GameObject[] enemiesOnScreen = GameObject.FindGameObjectsWithTag(currentEnemyTag);

            foreach (var enemyOnScreen in enemiesOnScreen)
            {
                // If player dies destroy all enemies from screen.
                Destroy(enemyOnScreen);
            }
        }
    }


    Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-m_spawningPositionLimitX, m_spawningPositionLimitX + 1);
        float randomY = Random.Range(-m_spawningPositionLimitY, m_spawningPositionLimitY + 1);

        return new Vector3(randomX, randomY, 0);
    }
    
}
