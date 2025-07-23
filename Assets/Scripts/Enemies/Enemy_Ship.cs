using UnityEngine;

public class Enemy_Ship : MonoBehaviour
{
    public EnemyStatsSO EnemyStats;
    private Transform m_playerPosition;
    private VisualEffects Vfx;


    [Header("Enemy AI Behaviour")]
    // The distance difference from the player.
    [SerializeField] private float limitOfDistanceFromPlayer;
    private float distanceFromThePlayer;

    private float minDistanceFromPlayer = 5.0f;
    private float maxDistanceFromPlayer = 15.0f;
    private Rigidbody2D m_rb;


    void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vfx = GetComponent<VisualEffects>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get a random original distance from the player.
        limitOfDistanceFromPlayer = GenerateDistanceFromPlayerLimit();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the position of the player each frame.
        Vector3 playerPosition = m_playerPosition.position;

        if (playerPosition != null)
        {
            FaceThePlayer(playerPosition);
            ChasePlayer(playerPosition);
        }
        
        Vfx.ThrustersEmitFire(m_rb.linearVelocity.sqrMagnitude);
        MaintainLinearVelocity();
    }

    void ChasePlayer(Vector3 targetPosition)
    {
        // Finds the distance from the player and maintains a constant distance
        if (targetPosition != null)
        {
            distanceFromThePlayer = Vector3.Distance(transform.position, targetPosition);
            Vector3 direction = (transform.position - targetPosition).normalized;

            if (distanceFromThePlayer < limitOfDistanceFromPlayer)
            {
                // Get away from the player
                m_rb.AddForce(direction * EnemyStats.movement_speed, ForceMode2D.Force);
            }
            else
            {
                // Go to the player's position
                m_rb.AddForceAtPosition(-direction * EnemyStats.movement_speed, targetPosition);
            }
        }

    }

    void FaceThePlayer(Vector3 target)
    {
        // Change the rotation accoriding to the player's rotation to always face the player
        Vector3 enemyRotation = target - transform.position;
        float rotationZ = Mathf.Atan2(enemyRotation.y, enemyRotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ + 90.0f), EnemyStats.rotation_speed * Time.deltaTime);
    }

    void MaintainLinearVelocity()
    {
        if (m_rb.linearVelocity.magnitude > EnemyStats.movement_speed)
        {
            m_rb.linearVelocity = m_rb.linearVelocity.normalized * EnemyStats.movement_speed;
        }
    }

    private float GenerateDistanceFromPlayerLimit()
    {
        return Mathf.FloorToInt(Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer));
    }
}
