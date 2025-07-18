using UnityEngine;

public class Charger : MonoBehaviour
{
    public EnemyStatsSO enemyStats;
    //Enemy Vitals
    private float m_health = 100f;

    private Transform m_player;

    private Rigidbody2D m_rb;

    private HealthBar m_health_bar;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_health_bar = GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        // Fill the health bar to maximum
        m_health_bar.UpdateStatusBar(m_health, enemyStats.maxHealth);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "laser_blue")
        {
            Destroy(collision.gameObject);
            TakeDamage(5.0f, collision);
        }

        if (collision.gameObject.tag == "Player")
        {
            //m_player.TakeDamage(enemyStats.damage_rate, collision);
        }

    }
    
    void TakeDamage(float damageAmount, Collision2D collision)
    {
        m_health -= damageAmount;
        m_health_bar.UpdateStatusBar(m_health, enemyStats.maxHealth);

        Vector2 contactOfdamage = collision.GetContact(0).point;
        GameObject damageEffect = Instantiate(enemyStats.taking_damage_VFX, contactOfdamage, Quaternion.identity);

        Destroy(damageEffect, 0.5f);

        if (m_health <= 0)
        {
            GameObject explosion = Instantiate(enemyStats.death_explosion_VFX, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(explosion, 2f);
        }
    }
}