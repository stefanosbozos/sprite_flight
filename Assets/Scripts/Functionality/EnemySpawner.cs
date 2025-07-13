using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameObject Player;
    private GameObject[] enemiesOnScreen;
    public List<WaveInfo> waves;
    private int enemyToSpawn;
    private int waveIndex = 0;


    // The spawning position limits on screen
    [SerializeField] private float min_limit_X = 1f;
    [SerializeField] private float max_limit_X = 1f;
    [SerializeField] private float min_limit_Y = 1f;
    [SerializeField] private float max_limit_Y = 1f;


    private float timer = 0f;

    [SerializeField] private GameObject spawnFX;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
        countEnemiesOnScreen();
    }

    void SpawnEnemy()
    {
        if (Player == null)
        {
            DestroyAllEnemiesOnScreen();
        }
        else
        {
            InstantiateEnemy();
        }
    }

    private void InstantiateEnemy()
    {
        timer += Time.deltaTime;
        // Check the spawning interval and how many enemies are on the screen.
        if (ReadyToSpawn())
        {
            enemyToSpawn = Random.Range(0, waves[waveIndex].enemiesToSpawn.Length);
            Vector3 position = RandomSpawningPosition();

            if (position != Player.transform.position)
            {
                GameObject spawnEffect = Instantiate(spawnFX, position, Quaternion.identity);
                Instantiate(waves[waveIndex].enemiesToSpawn[enemyToSpawn], position, Quaternion.identity);
                timer = 0f;
                Destroy(spawnEffect, 1f);
            }
        }
        else
        {
            PrepareNextWave();
        }
    }

    void PrepareNextWave()
    {
        // WaveInfo.timeBetweenWaves -= Time.deltaTime;
        // Debug.Log("Next wave in..." + Mathf.FloorToInt(timer));
        // if (WaveInfo.timeBetweenWaves <= 0f)
        // {
        // }
    }

    private bool ReadyToSpawn()
    {
        return
        timer >= waves[waveIndex].timeBetweenSpawns &&
        countEnemiesOnScreen() < waves[waveIndex].enemiesPerWave;
    }

    private void DestroyAllEnemiesOnScreen()
    {
        for (int i = 0; i < waves[waveIndex].enemiesToSpawn.Length; i++)
        {
            GameObject[] stillOnScreen = GameObject.FindGameObjectsWithTag(waves[waveIndex].enemiesToSpawn[i].tag);
            foreach (GameObject enemyOnScreen in stillOnScreen)
            {
                // If player dies destroy all enemies from screen.
                Destroy(enemyOnScreen);
            }
        }
        
    }

    Vector3 RandomSpawningPosition()
    {

        float randomX = Random.Range(min_limit_X, max_limit_X + 1);
        float randomY = Random.Range(min_limit_Y, max_limit_Y + 1);

        return new Vector3(randomX, randomY, 0);
    }

    int countEnemiesOnScreen()
    {
        int enemyCounter = 0;
        for (int i = 0; i < waves[waveIndex].enemiesToSpawn.Length; i++)
        {
            enemiesOnScreen = GameObject.FindGameObjectsWithTag(waves[waveIndex].enemiesToSpawn[i].tag);
            enemyCounter += enemiesOnScreen.Length;
        }
        return enemyCounter;
    }

}

[System.Serializable]
public class WaveInfo
{
    public GameObject[] enemiesToSpawn;
    public int enemiesPerWave = 1;
    // Give 5 seconds between each wave
    public static float timeBetweenWaves = 5f;
    //public float waveLength = 10f;
    public float timeBetweenSpawns = 3f;
}
