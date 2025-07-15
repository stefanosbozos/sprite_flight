using UnityEngine;

public class Creeper : MonoBehaviour
{
    public EnemyStatsSO enemyStats;
    private Player player;
    private Rigidbody2D rb;

    private float currentHealth = 100f;
    HealthBar floatingHealthBar;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        floatingHealthBar = GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        floatingHealthBar.UpdateStatusBar(currentHealth, enemyStats.maxHealth);
    }
    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (transform.position - player.transform.position).normalized;
            // Go to the player's position
            rb.AddForceAtPosition(-direction * enemyStats.movement_speed, player.transform.position);

            if (rb.linearVelocity.magnitude > enemyStats.movement_speed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * enemyStats.movement_speed;
            }

            // Change the rotation accoriding to the player's rotation to always face the player
            Vector3 enemyRotation = player.transform.position - transform.position;
            float rotationZ = Mathf.Atan2(enemyRotation.y, enemyRotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ + 90.0f), enemyStats.rotation_speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "laser_blue")
        {
            Destroy(collision.gameObject);
            TakeDamage(5.0f, collision);
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.TakeDamage(enemyStats.damage_rate, collision);
        }

    }
    
    void TakeDamage(float damageAmount, Collision2D collision)
    {
        currentHealth -= damageAmount;
        floatingHealthBar.UpdateStatusBar(currentHealth, enemyStats.maxHealth);

        Vector2 contactOfdamage = collision.GetContact(0).point;
        GameObject damageEffect = Instantiate(enemyStats.taking_damage_VFX, contactOfdamage, Quaternion.identity);

        Destroy(damageEffect, 0.5f);

        if (currentHealth <= 0)
        {
            GameObject explosion = Instantiate(enemyStats.death_explosion_VFX, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(explosion, 2f);
        }
    }
}
