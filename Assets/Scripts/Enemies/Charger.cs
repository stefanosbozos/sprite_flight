using UnityEngine;

public class Charger : MonoBehaviour
{
    public EnemyStatsSO enemyStats;
    //Enemy Vitals
    private float m_health = 100f;

    private Transform m_player;

    private Rigidbody2D m_rb;

    private HealthBar m_health_bar;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_health_bar = GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        // Fill the health bar to maximum
        m_health_bar.UpdateStatusBar(m_health, enemyStats.maxHealth);
    }

    
}