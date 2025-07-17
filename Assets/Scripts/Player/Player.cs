using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerStatsSO PlayerStats;
    public Projectile Projectile;
    public GameObject gunMuzzles;
    

    // Player Input
    private InputAction m_movePlayer;
    private InputAction m_shootLaser;
    private InputAction m_aim;

    // Physics and Movement
    private Vector2 m_moveValue;
    private int rotationOffset = 90;
    private Rigidbody2D rb;


    // Player Vitals
    private float m_currentHealth;
    private float m_currentShieldAmount;
    private bool isShieldActive;


    // Player's firing system
    public float TimeBetweenShots;
    private float m_timeSinceLastShot = 0f;


    // Laser Heating Mechanic
    public float LaserCooldownInterval = 5f;
    public float LaserCooldownDecreatingStep = 3f;
    public float LaserHeatIncreaseStep = 3f;

    private float m_laserTemperature = 0f;
    private const float k_laserHeatLimit = 100f;

    
    // Player Particle System
    public ParticleSystem ThrustersFX;
    public ParticleSystem shipSmoke;


    // Pause System
    private PauseSystem pauseSystem;


    void Awake()
    {
        pauseSystem = GameObject.FindGameObjectWithTag("game_manager").GetComponent<PauseSystem>();
        
        rb = GetComponent<Rigidbody2D>();
        
        //Keyboard support input
        m_movePlayer = InputSystem.actions.FindAction("Move");
        m_shootLaser = InputSystem.actions.FindAction("Shoot");
        m_aim = InputSystem.actions.FindAction("Aim");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Player state
        m_currentHealth = 100;
        m_currentShieldAmount = 0;
        gunMuzzles.SetActive(false);
    }


    // Update is called once per frame 
    void Update()
    {
        if (!pauseSystem.GameIsPaused())
        {
            PlayerMovement();
            playerVisualEffects();
            ShootProjectile();
            Aim();
        }
    }


    void PlayerMovement()
    {
        m_moveValue = m_movePlayer.ReadValue<Vector2>().normalized;
        rb.AddForce(m_moveValue * PlayerStats.ThrustForce);

        // This is to stop the player for accelerating if the move button is constantly pressed.
        if ( rb.linearVelocity.magnitude > PlayerStatsSO.k_MaxSpeed )
        {
            rb.linearVelocity = rb.linearVelocity.normalized * PlayerStatsSO.k_MaxSpeed;
        }
    }


    void Aim()
    {
        Vector3 mouseScreenPos = m_aim.ReadValue<Vector2>();  
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));

        Vector3 playerDirection = mouseWorldPos - transform.position;
        playerDirection.z = 0; // Ensure that the player does not rotate on the Z-axis

        if ( playerDirection.sqrMagnitude > 0.01f )
        {
            float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - rotationOffset);
        }
    }

    void ShootProjectile()
    {
        // Keep track of the time that the lasers are not fired.
        m_timeSinceLastShot += Time.deltaTime;

        if ( m_shootLaser.inProgress )
        {
            if ( m_timeSinceLastShot >= TimeBetweenShots )
            {
                if ( m_laserTemperature < k_laserHeatLimit )
                {
                    Projectile.FireProjectileAt( transform.Find("ProjectileSpawn").position, transform.Find("ProjectileSpawn").rotation );
                    m_laserTemperature += LaserHeatIncreaseStep;
                }
                m_timeSinceLastShot = 0f;
            }
        }

        CheckGunsTemperature();
    }

    void CheckGunsTemperature()
    {
        if ( m_laserTemperature >= 0 && m_laserTemperature < k_laserHeatLimit )
        {
            m_laserTemperature -= Time.deltaTime * LaserCooldownDecreatingStep;
        }

        if ( m_laserTemperature >= k_laserHeatLimit )
        {
            CooldownLaser();
        }
    }

    void CooldownLaser()
    {
        LaserCooldownInterval -= Time.deltaTime;

        if ( m_laserTemperature >= k_laserHeatLimit && LaserCooldownInterval <= 0.0f )
        {
            m_laserTemperature = 0f;
            LaserCooldownInterval = 3f;
        }
    }

    void playerVisualEffects()
    {

        if ( m_moveValue.sqrMagnitude > 0 )
        {
            var emission = ThrustersFX.emission;
            emission.rateOverTime = 100;
        }
        else
        {
            var emission = ThrustersFX.emission;
            emission.rateOverTime = 0;
        }

        if ( m_shootLaser.inProgress && m_laserTemperature < k_laserHeatLimit )
        {
            gunMuzzles.SetActive(true);
        }
        else
        {
            gunMuzzles.SetActive(false);
        }

        // Play smoke if player below 20% health
        if ( m_currentHealth <= 20 )
        {
            shipSmoke.Play();
        }
        else
        {
            shipSmoke.Stop();
            shipSmoke.Clear();
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

        if ( collision != null )
        {
            Vector2 contactOfdamage = collision.GetContact(0).point;
            GameObject damageEffect = Instantiate(PlayerStats.TakeDamageFX, contactOfdamage, Quaternion.identity);
            Destroy(damageEffect, 0.5f);
        }

        // Player's is dead.
        if ( m_currentHealth <= 0 )
        {
            KillPlayer();
        }
    }
    
    void ActivateShields()
    {
        isShieldActive = !isShieldActive;
        m_currentShieldAmount = 100;
    }

    void KillPlayer()
    {
        GameObject OnDeathExplosion = Instantiate(PlayerStats.DeathFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(OnDeathExplosion, 3f);
    }

    public bool GunsOverheated()
    {
        return m_laserTemperature >= k_laserHeatLimit ? true : false;
    }

    public float GunsTemperature => m_laserTemperature;
    
}





/*  FURTHER NOTES
    1. The trhustForce is being multiplied by the direction which is the result of subtracting the mousePos from the position of the player object
    the further the mouse click the bigger the result. This causes the speed to vary based on how far the mouse is clicked from the player object in
    the world. This is why we need to follow the technique called "normalization". 

    Normalization keeps direction the same but limits its length to 1.

*/

// Old functionality scripts
//void MoveToDirection()
    // {

    //     // This script moves the player towards where the mouse is pointing.
    //     // Get the mouse screen position, translate that position to world position and put it in a Vector3
    //     // https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Camera.ScreenToWorldPoint.html
    //     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

    //     // Calculate the direction to the mouse and normalize it (1)
    //     Vector2 direction = (mousePos - transform.position).normalized;

    //     // Set the Player game object to face this direction - https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Transform-up.html
    //     transform.up = direction;
        
    //     //Add force towards the direction of the mouse click location(1)
    //     rb.AddForce(direction * thrustForce);

    //     if (rb.linearVelocity.magnitude > maxSpeed)
    //     {
    //         rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    //     }
    // }
