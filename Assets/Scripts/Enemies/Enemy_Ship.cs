using UnityEngine;

public class Enemy_Ship : MonoBehaviour
{
    public EnemyStatsSO EnemyStats;


    private Transform m_player;
    private Projectile m_projectile;
    private Rigidbody2D m_rb;
    private float m_currentHealth = 100f;
    HealthBar floatingHealthBar;


    [Header("Enemy Shooting System")]
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
    [SerializeField] private ParticleSystem ThrusterVFX;

    void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_rb = GetComponent<Rigidbody2D>();
        m_projectile = EnemyStats.ProjectilePreFab.GetComponent<Projectile>();

        floatingHealthBar = GetComponentInChildren<HealthBar>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        limitOfDistanceFromPlayer = Mathf.FloorToInt(Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer));
        timeBetweenShots = Random.Range(minTimeToAttack, maxTimeToAttack);
        floatingHealthBar.UpdateStatusBar(m_currentHealth, EnemyStats.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
        Shoot();
        ThrusterEmission();
    }

    void ChasePlayer()
    {
        if (m_player != null)
        {
            distanceFromThePlayer = Vector3.Distance(transform.position, m_player.transform.position);
            Vector3 direction = (transform.position - m_player.transform.position).normalized;
            if (distanceFromThePlayer < limitOfDistanceFromPlayer)
            {
                // Get away from the player
                m_rb.AddForce(direction * EnemyStats.movement_speed, ForceMode2D.Force);
            }
            else
            {
                // Go to the player's position
                m_rb.AddForceAtPosition(-direction * EnemyStats.movement_speed, m_player.transform.position);
            }

            if (m_rb.linearVelocity.magnitude > EnemyStats.movement_speed)
            {
                m_rb.linearVelocity = m_rb.linearVelocity.normalized * EnemyStats.movement_speed;
            }

            // Change the rotation accoriding to the player's rotation to always face the player
            Vector3 enemyRotation = m_player.transform.position - transform.position;
            float rotationZ = Mathf.Atan2(enemyRotation.y, enemyRotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ + 90.0f), EnemyStats.rotation_speed * Time.deltaTime);
        }
        
    }

    void Shoot()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenShots)
        {
            // Check the state of the enemy. As we do not want more than 3 enemies attach at the same time.
            m_projectile.FireProjectileAt( transform.Find("ProjectileSpawn").position, transform.Find("ProjectileSpawn").rotation );
            timer = 0;
        }
    }

    void ThrusterEmission()
    {
        if (m_rb.linearVelocity.sqrMagnitude > 0.01)
        {
            var emission = ThrusterVFX.emission;
            emission.rateOverTime = 30;
        }
        if (m_rb.linearVelocity.sqrMagnitude <= 0)
        {
            var emission = ThrusterVFX.emission;
            emission.rateOverTime = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "laser_blue")
        {

            Projectile projectile = collision.gameObject.GetComponentInParent<Projectile>();

            if (projectile != null)
            {
                Destroy(collision.gameObject);
                TakeDamage(projectile.GetDamageAmount, collision);
            }

        }
    }

    void TakeDamage(float damageAmount, Collision2D collision)
    {
        m_currentHealth -= damageAmount;
        floatingHealthBar.UpdateStatusBar(m_currentHealth, EnemyStats.maxHealth);

        Vector2 contactOfdamage = collision.GetContact(0).point;
        GameObject damageEffect = Instantiate(EnemyStats.taking_damage_VFX, contactOfdamage, Quaternion.identity);

        Destroy(damageEffect, 0.5f);

        if (m_currentHealth <= 0)
        {
            GameObject explosion = Instantiate(EnemyStats.death_explosion_VFX, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(explosion, 2f);
        }
    }
}
