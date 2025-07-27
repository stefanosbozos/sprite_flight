using UnityEngine;

public abstract class Player : MonoBehaviour
{
    private const float k_MaxSpeed = 100f;

    protected Rigidbody2D m_rigidBody;
    protected VisualEffects m_vfx;

    protected float m_health;
    protected float m_shield;
    protected int m_maxHealth = 100;
    protected int m_maxShield = 50;

    protected void MaintainLinearVelocity()
    {
        // This is to stop the player for accelerating if the move button is constantly pressed.
        if (m_rigidBody.linearVelocity.magnitude > k_MaxSpeed)
        {
            m_rigidBody.linearVelocity = m_rigidBody.linearVelocity.normalized * k_MaxSpeed;
        }
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

    private void TakeDamage(float damageAmount, Collision2D collision = null)
    {
        if (ShieldActive())
        {
            m_shield -= damageAmount;
        }
        else
        {
            m_health -= damageAmount;
        }

        m_vfx.EmitSmoke(IsInCriticalState());

        if (collision != null)
        {
            Vector2 contactOfdamage = collision.GetContact(0).point;
            m_vfx.TakeDamageVFX(contactOfdamage, Quaternion.identity);
        }

        // Player's is dead.
        if (IsDead())
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        m_vfx.ExplodeVFX(transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private bool IsInCriticalState()
    {
        return m_health <= 20;
    }

    private bool ShieldActive()
    {
        return m_shield > 0;
    }

    public bool IsDead()
    {
        return m_health <= 0;
    }

    public float Health => m_health;
    public float MaxHealth => m_maxHealth;
    public float Shield => m_shield;
    public float MaxShield => m_maxShield;
}