using UnityEngine;

public class Spawner : MonoBehaviour
{
    // This array holds all the enemies that the player can encounter.
    public GameObject[] enemy;
    private GameObject Player;
    private GameObject[] enemiesOnScreen;
    public float timeBetweenSpawns = 2f;
    [SerializeField]
    private int enemyLimitOnScreen = 10;
    [SerializeField]
    private float min_limit_X = 1f;
    [SerializeField]
    private float max_limit_X = 1f;
    [SerializeField]
    private float min_limit_Y = 1f;
    [SerializeField]
    private float max_limit_Y = 1f;


    private float timer = 0f;

    Camera m_Camera;

    [SerializeField] private GameObject spawnFX;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

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
            GameObject[] stillOnScreen = GameObject.FindGameObjectsWithTag(enemy[0].tag);
            foreach(GameObject enemyOnScreen in stillOnScreen)
            {
                // If player dies destroy all enemies from screen.
                Destroy(enemyOnScreen);
            }
        }
        timer += Time.deltaTime;
        // Check the spawning interval and how many enemies are on the screen.
        if (timer >= timeBetweenSpawns && countEnemiesOnScreen() < enemyLimitOnScreen)
        {
            Vector3 position = RandomSpawningPosition();
            if (Player != null)
            {
                if (position != Player.transform.position)
                {
                    GameObject spawnEffect = Instantiate(spawnFX, position, Quaternion.identity);
                    Instantiate(enemy[0], position, Quaternion.identity);
                    timer = 0f;
                    Destroy(spawnEffect, 1f);
                }
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
        enemiesOnScreen = GameObject.FindGameObjectsWithTag("enemy_ship");
        return enemiesOnScreen.Length;
    }

}
