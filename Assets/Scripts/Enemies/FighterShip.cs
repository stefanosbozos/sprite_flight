using UnityEngine;

public class FighterShip : Enemy, ISpaceship, ICanAttack
{

    // The distance difference from the player.
    private float limitOfDistanceFromPlayer;
    private float distanceFromThePlayer;
    protected float m_attackTimer;
    [SerializeField] protected float m_timeBetweenAttacks;


    // Fighter Ship's Shooting system
    private Projectile m_projectile;
    private float m_thrusterEmmissionRate = 5f;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_vfx = GetComponent<VisualEffects>();
        m_projectile = EnemyStats.ProjectilePreFab.GetComponent<Projectile>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get a random original distance from the player.
        limitOfDistanceFromPlayer = GenerateDistanceFromPlayerLimit();
        m_timeBetweenAttacks = Random.Range(ICanAttack.minTimeToAttack, ICanAttack.maxTimeToAttack);
    }

    // Update is called once per frame
    void Update()
    {
        // Get the position of the player each frame.
        if (m_playerPosition != null)
        {
            EngageWithPlayer();
            Thrust();
            Attack();
            base.FacePlayer();
            base.MaintainLinearVelocity();
            base.AvoidOtherEnemies();
        }
    }

    public void EngageWithPlayer()
    {
        // This enemy keeps distance from the player and shoots
        // Finds the distance from the player and maintains a constant distance
        if (m_playerPosition.position != null)
        {
            distanceFromThePlayer = Vector3.Distance(transform.position, m_playerPosition.position);
            Vector3 direction = (transform.position - m_playerPosition.position).normalized;

            if (distanceFromThePlayer < limitOfDistanceFromPlayer)
            {
                // Get away from the player
                m_rigidbody.AddForce(direction * EnemyStats.movement_speed, ForceMode2D.Force);
            }
            else
            {
                // Go to the player's position
                m_rigidbody.AddForceAtPosition(-direction * EnemyStats.movement_speed, m_playerPosition.position);
            }
        }

    }

    private float GenerateDistanceFromPlayerLimit()
    {
        return Mathf.FloorToInt(Random.Range(ISpaceship.minDistanceFromPlayer, ISpaceship.maxDistanceFromPlayer));
    }

    public void Attack()
    {
        m_attackTimer += Time.deltaTime;
        if (IsReadyToAttack())
        {
            m_projectile.FireProjectileAt(transform.Find("ProjectileSpawn").position, transform.Find("ProjectileSpawn").rotation);
            m_attackTimer = 0;
        }
    }

    public void Thrust()
    {
        m_vfx.ThrustersEmitFire(m_rigidbody.linearVelocity.sqrMagnitude * m_thrusterEmmissionRate);
    }

    public bool IsReadyToAttack()
    {
        return m_attackTimer > m_timeBetweenAttacks;
    }

    protected override void TakeDamage()
    {
        m_vfx.ExplodeVFX(transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
    
