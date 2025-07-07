using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private GameObject[] EnemiesOnScreen;

    private Rigidbody2D rb;

    [Header("Enemy Shooting System")]
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float timeBetweenShots = 1f;
    private float timer = 0f;
    private bool isAttacking;


    [Header("Enemy Movement")]
    [SerializeField]
    private float movement_speed = 0.2f;
    [SerializeField]
    private float rotation_speed = 5f;

    // The distance difference from the player.
    private float deltaDistance;
    private Vector3 MAX_DISTANCE_BETWEEN_ENEMIES = new Vector3(4f, 4f, 0f);


    [SerializeField]
    private GameObject deathExplosionFX;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        deltaDistance = Mathf.FloorToInt(Random.Range(5f, 20f));
        EnemiesOnScreen = GameObject.FindGameObjectsWithTag("enemy_ship");
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        Shoot();
        AvoidOtherEnemies();
    }

    void FollowPlayer()
    {
        // Go to the players position (Lurker's behaviour)
        Vector3 distanceFromThePlayer = new Vector3(transform.position.x, transform.position.y - deltaDistance, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position - distanceFromThePlayer, movement_speed * Time.deltaTime);

        // Change the rotation accoriding to the player's rotation to always face the player
        Vector3 enemyRotation = player.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(enemyRotation.y, enemyRotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ + 90.0f), rotation_speed * Time.deltaTime);
    }

    void AvoidOtherEnemies()
    {
        foreach (GameObject enemy in EnemiesOnScreen)
        {
            if (enemy != null)
            {
                Vector3 distanceBetweenEnemies = transform.position - enemy.transform.position;

                // Debug.Log("enemy pos: " + transform.position);
                // Debug.Log("other enemy position: " + enemy.transform.position);
                // Debug.Log("Distance between them: " + distanceBetweenEnemies);

                // if (
                //     (distanceBetweenEnemies.x <= MAX_DISTANCE_BETWEEN_ENEMIES.x) &&
                //     (distanceBetweenEnemies.y <= MAX_DISTANCE_BETWEEN_ENEMIES.y)
                //     )
                // {
                //     transform.position = Vector3.one
                // }
            }
        }
    }

    void Shoot()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenShots)
        {
            // Check the state of the enemy. As we do not want more than 3 enemies attach at the same time.
            isAttacking = !isAttacking;
            GameObject spawnedBullet = Instantiate(projectile, transform.Find("ProjectileSpawn").position, transform.Find("ProjectileSpawn").rotation);
            timer = 0;
            Destroy(spawnedBullet, 3f);
        }

        isAttacking = !isAttacking;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "laser_blue")
        {
            GameObject explosion = Instantiate(deathExplosionFX, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            Destroy(gameObject);
            Destroy(explosion, 2f);
        }
    }
}
