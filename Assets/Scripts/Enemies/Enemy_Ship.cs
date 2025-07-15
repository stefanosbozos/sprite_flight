using UnityEngine;

public class Enemy_Ship : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO m_enemyStats;
    private Transform player;
    private Rigidbody2D rb;

    [Header("Enemy Vitals")]
    private float currentHealth = 100f;
    HealthBar floatingHealthBar;


    [Header("Enemy Shooting System")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float minTimeToAttack = 1.5f;
    [SerializeField] private float maxTimeToAttack = 5.0f;
    [SerializeField] private float timeBetweenShots;
    private float timer = 0f;


    [Header("Enemy Behaviour")]
    // The distance difference from the player.
    private float distanceFromThePlayer;

    [SerializeField] private float limitOfDistanceFromPlayer;
    private float minDistanceFromPlayer = 5.0f;
    private float maxDistanceFromPlayer = 15.0f;


    [Header("Enemy VFX")]
    [SerializeField] private ParticleSystem enemyThrusterVFX;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        floatingHealthBar = GetComponentInChildren<HealthBar>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        limitOfDistanceFromPlayer = Mathf.FloorToInt(Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer));
        timeBetweenShots = Random.Range(minTimeToAttack, maxTimeToAttack);
        floatingHealthBar.UpdateStatusBar(currentHealth, m_enemyStats.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
        Shoot();
        ThrusterVFX();
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            distanceFromThePlayer = Vector3.Distance(transform.position, player.transform.position);
            Vector3 direction = (transform.position - player.transform.position).normalized;
            if (distanceFromThePlayer < limitOfDistanceFromPlayer)
            {
                // Get away from the player
                rb.AddForce(direction * m_enemyStats.movement_speed, ForceMode2D.Force);
            }
            else
            {
                // Go to the player's position
                rb.AddForceAtPosition(-direction * m_enemyStats.movement_speed, player.transform.position);
            }

            if (rb.linearVelocity.magnitude > m_enemyStats.movement_speed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * m_enemyStats.movement_speed;
            }

            // Change the rotation accoriding to the player's rotation to always face the player
            Vector3 enemyRotation = player.transform.position - transform.position;
            float rotationZ = Mathf.Atan2(enemyRotation.y, enemyRotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ + 90.0f), m_enemyStats.rotation_speed * Time.deltaTime);
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
        if (rb.linearVelocity.sqrMagnitude > 0.01)
        {
            enemyThrusterVFX.emissionRate = 30;
        }
        if (rb.linearVelocity.sqrMagnitude <= 0)
        {
            enemyThrusterVFX.emissionRate = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "laser_blue")
        {
            Destroy(collision.gameObject);
            TakeDamage(25.0f, collision);
        }
    }

    void TakeDamage(float damageAmount, Collision2D collision)
    {
        currentHealth -= damageAmount;
        floatingHealthBar.UpdateStatusBar(currentHealth, m_enemyStats.maxHealth);

        Vector2 contactOfdamage = collision.GetContact(0).point;
        GameObject damageEffect = Instantiate(m_enemyStats.taking_damage_VFX, contactOfdamage, Quaternion.identity);

        Destroy(damageEffect, 0.5f);

        if (currentHealth <= 0)
        {
            GameObject explosion = Instantiate(m_enemyStats.death_explosion_VFX, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(explosion, 2f);
        }
    }
}
