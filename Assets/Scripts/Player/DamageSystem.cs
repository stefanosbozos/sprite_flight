using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;
    private float m_currentHealth;

    private PlayerVFX m_playerVFX;

    void Awake()
    {
        m_playerVFX = GetComponent<PlayerVFX>();
    }

    void Start()
    {
        m_currentHealth = 100;
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "enemy_fire")
        {
            Projectile enemyProjectile = collision.gameObject.GetComponent<Projectile>();

            if (enemyProjectile != null)
            {
                TakeDamage(enemyProjectile.GetDamageAmount, collision);
                Destroy(collision.gameObject);
            }

        }
        
    }

    public void TakeDamage(float damageAmount, Collision2D collision=null)
    {
        m_currentHealth -= damageAmount;
        m_playerVFX.EmitSmoke(m_currentHealth <= 20);
        
        if (collision != null)
        {
            Vector2 contactOfdamage = collision.GetContact(0).point;
            m_playerVFX.SparksVFX(contactOfdamage, Quaternion.identity);
        }

        // Player's is dead.
        if (m_currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        m_playerVFX.ExplodeVFX(transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}