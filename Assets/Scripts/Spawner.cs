using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // This array holds all the enemies that the player can encounter.
    public GameObject[] enemy;
    public GameObject Player;
    private GameObject[] enemiesOnScreen;
    [SerializeField] private float timeBetweenSpawns = 2f;
    [SerializeField] private int enemyLimitOnScreen = 10;
    private float timer = 0f;

    [SerializeField] private GameObject spawnFX;

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
        countEnemiesOnScreen();
    }

    void SpawnEnemy()
    {
        timer += Time.deltaTime;
        // Check the spawning interval and how many enemies are on the screen.
        if (timer >= timeBetweenSpawns && countEnemiesOnScreen() <= enemyLimitOnScreen)
        {
            Vector3 position = RandomSpawningPosition();
            if (position != Player.transform.position)
            {
                GameObject spawnEffect = Instantiate(spawnFX, position, Quaternion.identity);
                Instantiate(enemy[0], position, Quaternion.identity);
                timer = 0f;
                Destroy(spawnEffect, 1f);
            }
        }
    }

    Vector3 RandomSpawningPosition()
    {
        float randomX = Random.Range(-12, 13);
        float randomY = Random.Range(-6.5f, 6.5f);

        return new Vector3(randomX, randomY, 0);
    }

    int countEnemiesOnScreen()
    {
        enemiesOnScreen = GameObject.FindGameObjectsWithTag(enemy[0].tag);
        return enemiesOnScreen.Length;
    }

}
