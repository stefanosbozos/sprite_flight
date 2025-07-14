using UnityEngine;

public class Enemy_Ship : MonoBehaviour
{
    private GameObject player;
    private GameObject[] EnemiesOnScreen;
    private Rigidbody2D rb;

    [Header("Enemy Shooting System")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float minTimeToAttack = 1.5f;
    [SerializeField] private float maxTimeToAttack = 5.0f;
    [SerializeField] private float timeBetweenShots;
    private float timer = 0f;


    [Header("Enemy Movement")]
    [SerializeField] private float movement_speed = 3f;
    [SerializeField] private float rotation_speed = 5f;

    [Header("Enemy Behaviour")]
    // The distance difference from the player.
    private float distanceFromThePlayer;

    [SerializeField] private float limitOfDistanceFromPlayer;
    private float minDistanceFromPlayer = 5.0f;
    private float maxDistanceFromPlayer = 15.0f;
    private float distanceFromEnemies;
    [SerializeField] private float distanceBetweenEnemies = 5.0f;


    [Header("Enemy VFX")]
    [SerializeField] private GameObject deathExplosionFX;
    [SerializeField] private ParticleSystem enemyThrusterVFX;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        EnemiesOnScreen = GameObject.FindGameObjectsWithTag("enemy_ship");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        limitOfDistanceFromPlayer = Mathf.FloorToInt(Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer));
        timeBetweenShots = Random.Range(minTimeToAttack, maxTimeToAttack);
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        //Shoot();
        AvoidOtherEnemies();
        ThrusterVFX();
    }

    void FollowPlayer()
    {
        distanceFromThePlayer = Vector3.Distance(transform.position, player.transform.position);
        Vector3 direction = (transform.position - player.transform.position).normalized;
        if (distanceFromThePlayer < limitOfDistanceFromPlayer)
        {
            // Get away from the player
            rb.AddForce(direction * movement_speed,ForceMode2D.Force);
        }
        else
        {
            // Go to the player's position
            rb.AddForceAtPosition(-direction * movement_speed, player.transform.position);
        }

        if (rb.linearVelocity.magnitude > movement_speed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * movement_speed;
        }

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
                distanceFromEnemies = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceFromEnemies < distanceBetweenEnemies)
                {
                    // Stay away from other enemies
                    Vector3 enemyDirection = (transform.position - enemy.transform.position).normalized;
                    rb.AddForce(enemyDirection * movement_speed);
                }
            }
        }
    }

    void Shoot()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenShots)
        {
            // Check the state of the enemy. As we do not want more than 3 enemies attach at the same time.
            GameObject spawnedBullet = Instantiate(projectile, transform.Find("ProjectileSpawn").position, transform.Find("ProjectileSpawn").rotation);
            timer = 0;
            Destroy(spawnedBullet, 3f);
        }
    }

    void ThrusterVFX()
    {
        if (rb.linearVelocity.x > 0.1)
        {
            enemyThrusterVFX.emissionRate = 30;
        }
        if (rb.linearVelocity.x <= 0)
        {
            enemyThrusterVFX.emissionRate = 0;
        }
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
