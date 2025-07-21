using UnityEngine;

public class Creeper : MonoBehaviour
{
    public EnemyStatsSO enemyStats;
    private Transform player;
    private Rigidbody2D rb;

    private float currentHealth = 100f;
    HealthBar floatingHealthBar;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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

    void OnCollisionStay2D(Collision2D collision)
    {
        // if (collision.gameObject.tag == "Player")
        // {
        //     player.TakeDamage(enemyStats.damage_rate, collision);
        // }
    }
    
}
