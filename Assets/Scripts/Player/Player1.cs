using UnityEngine;

public class Player1 : MonoBehaviour
{
    public PlayerStatsSO PlayerStats;


    private PlayerMovement m_playerMovement;
    private PlayerAiming m_playerAiming;
    private PlayerShooting m_playerShooting;
    private PauseSystem m_pauseSystem;
    private GameObject m_gameManager;


    private float m_currentHealth;

    private PlayerVFX m_playerVFX;


    void Awake()
    {
        m_gameManager = GameObject.FindGameObjectWithTag("game_manager");
        m_pauseSystem = m_gameManager.GetComponent<PauseSystem>();

        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerAiming = GetComponent<PlayerAiming>();
        m_playerShooting = GetComponent<PlayerShooting>();

        m_playerVFX = GetComponent<PlayerVFX>();
    }

    void Start()
    {
        m_currentHealth = 100;
    }

    void Update()
    {
        if (!m_pauseSystem.GameIsPaused())
        {
            m_playerMovement.Move();
            m_playerAiming.Aim();
            m_playerShooting.Shoot();
        }
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
            GameObject damageEffect = Instantiate(PlayerStats.TakeDamageFX, contactOfdamage, Quaternion.identity);
            Destroy(damageEffect, 0.5f);
        }

        // Player's is dead.
        if (m_currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        GameObject OnDeathExplosion = Instantiate(PlayerStats.DeathFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(OnDeathExplosion, 3f);
    }

}