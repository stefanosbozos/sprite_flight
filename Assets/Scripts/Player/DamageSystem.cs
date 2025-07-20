using Unity.VisualScripting;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;
    private VisualEffects m_playerVFX;

    void Awake()
    {
        m_playerVFX = GetComponent<VisualEffects>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "enemy_fire")
        {
            Projectile enemyProjectile = collision.gameObject.GetComponent<Projectile>();

            if (enemyProjectile != null)
            {
                TakeDamage(enemyProjectile.GetDamage(), collision);
                Destroy(collision.gameObject);
            }

        }

    }

    public void TakeDamage(float damageAmount, Collision2D collision=null)
    {
        if (m_playerStats.ShieldActive())
        {
            m_playerStats.DamageToShield(damageAmount);
            Debug.Log(m_playerStats.Shield);
        }
        else
        {
            m_playerStats.DamageToHealth(damageAmount);
            Debug.Log(m_playerStats.Health);
        }

        m_playerVFX.EmitSmoke(m_playerStats.IsInCriticalState());
        
        if (collision != null)
        {
            Vector2 contactOfdamage = collision.GetContact(0).point;
            m_playerVFX.SparksVFX(contactOfdamage, Quaternion.identity);
        }

        // Player's is dead.
        if (m_playerStats.IsDead())
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