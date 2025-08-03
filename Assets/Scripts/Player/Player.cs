using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;
    protected Rigidbody2D m_rigidBody;
    protected VisualEffects m_vfx;
    

    void Start()
    {
        m_playerStats.SetShield(100);
        m_playerStats.SetHealth(100);
    }

    protected void MaintainLinearVelocity()
    {
        // This is to stop the player for accelerating if the move button is constantly pressed.
        if (m_rigidBody.linearVelocity.magnitude > m_playerStats.MaxSpeed)
        {
            m_rigidBody.linearVelocity = m_rigidBody.linearVelocity.normalized * m_playerStats.MaxSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "enemy_fire")
        {
            Projectile enemyProjectile = collision.gameObject.GetComponent<E_Laser>();

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
            m_playerStats.DecreaseShield(damageAmount);
        }
        else
        {
            m_playerStats.DecreaseHealth(damageAmount);
        }

        // m_vfx.EmitSmoke(IsInCriticalState());

        if (collision != null)
        {
            Vector2 contactOfdamage = collision.GetContact(0).point;
            // m_vfx.TakeDamageVFX(contactOfdamage, Quaternion.identity);
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
        return m_playerStats.Health <= 20;
    }

    private bool ShieldActive()
    {
        return m_playerStats.Shield > 0;
    }

    public bool IsDead()
    {
        return m_playerStats.Health <= 0;
    }

}