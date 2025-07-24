using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public EnemyStatsSO EnemyStats;
    public EnemyRuntimeSetSO EnemyRuntimeSet;
    protected Transform m_playerPosition;
    protected Rigidbody2D m_rigidbody;

    // Fighter Ship's Visual Effects
    protected VisualEffects m_vfx;

    [SerializeField] private float m_distanceBetweenEnemies;
    private float m_distanceFromOtherEnemies;

    void OnEnable()
    {
        EnemyRuntimeSet.Add(gameObject);
        m_distanceBetweenEnemies = Random.Range(EnemyStats.minDistanceFromOtherEnemies, EnemyStats.maxDistanceFromOtherEnemies);
    }

    void OnDestroy()
    {
        EnemyRuntimeSet.Remove(gameObject);
    }


    protected void FacePlayer()
    {
        if (m_playerPosition != null)
        {
            // Change the rotation accoriding to the player's rotation to always face the player
            Vector3 enemyRotation = m_playerPosition.position - transform.position;
            float rotationZ = Mathf.Atan2(enemyRotation.y, enemyRotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ + 90.0f), EnemyStats.rotation_speed * Time.deltaTime);
        }
    }

    protected void MaintainLinearVelocity()
    {
        if (m_rigidbody.linearVelocity.magnitude > EnemyStats.movement_speed)
        {
            m_rigidbody.linearVelocity = m_rigidbody.linearVelocity.normalized * EnemyStats.movement_speed;
        }
    }

    protected void AvoidOtherEnemies()
    {
        foreach (var enemy in EnemyRuntimeSetSO.Enemies)
        {
            if (enemy != null)
            {
                m_distanceFromOtherEnemies = Vector3.Distance(transform.position, enemy.transform.position);
                if (m_distanceFromOtherEnemies < m_distanceBetweenEnemies)
                {
                    // Stay away from other enemies
                    Vector3 enemyDirection = (transform.position - enemy.transform.position).normalized;
                    m_rigidbody.AddForce(enemyDirection * EnemyStats.movement_speed);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "player_fire")
        {

            Projectile projectile = collision.gameObject.GetComponentInParent<Projectile>();

            if (projectile != null)
            {
                Destroy(collision.gameObject);
                TakeDamage();
            }

        }
    }

    protected abstract void TakeDamage();

}