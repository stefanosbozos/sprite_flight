using UnityEngine;

public class AvoidOtherEnemies : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO enemyStats;
    [SerializeField] private float m_distanceBetweenEnemies;
    private float m_distanceFromOtherEnemies;
    private GameObject[] m_EnemiesOnScreen;
    private Rigidbody2D rb;

    void Awake()
    {
        m_EnemiesOnScreen = GameObject.FindGameObjectsWithTag("enemy");
        rb = GetComponent<Rigidbody2D>();
    }

    void AvoidEnemies()
    {
        foreach (GameObject enemy in m_EnemiesOnScreen)
        {
            if (enemy != null)
            {
                m_distanceFromOtherEnemies = Vector3.Distance(transform.position, enemy.transform.position);
                if (m_distanceFromOtherEnemies < m_distanceBetweenEnemies)
                {
                    // Stay away from other enemies
                    Vector3 enemyDirection = (transform.position - enemy.transform.position).normalized;
                    rb.AddForce(enemyDirection * enemyStats.movement_speed);
                }
            }
        }
    }

    void Update()
    {
        AvoidEnemies();
    }
}