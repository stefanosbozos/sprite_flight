using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public EnemyStatsSO EnemyStats;
    public EnemyAISO EnemyAI;
    public EnemyRuntimeSetSO EnemyRuntimeSet;
    protected Transform m_targetPosition;
    protected Rigidbody2D m_rb;
    protected HealthBar floatingHealthBar;
    protected VisualEffects Vfx;


    void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_targetPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        floatingHealthBar = GetComponentInChildren<HealthBar>();
        Vfx = GetComponent<VisualEffects>();
    }

    void Update()
    {
        FaceThePlayer(m_targetPosition.position);
        MaintainLinearVelocity();
    }

    void OnEnable()
    {
        // Keep track of how many enemies are on screen
        EnemyRuntimeSet.Add(gameObject);
    }

    void OnDestroy()
    {
        // Keep track of how many enemies are on screen
        EnemyRuntimeSet.Remove(gameObject);
    }

    protected abstract void FollowPlayer(Vector3 targetPosition);

    protected void FaceThePlayer(Vector3 target)
    {
        // Change the rotation accoriding to the player's rotation to always face the player
        Vector3 enemyRotation = target - transform.position;
        float rotationZ = Mathf.Atan2(enemyRotation.y, enemyRotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ + 90.0f), EnemyStats.rotation_speed * Time.deltaTime);
    }

    protected void MaintainLinearVelocity()
    {
        if (m_rb.linearVelocity.magnitude > EnemyStats.movement_speed)
        {
            m_rb.linearVelocity = m_rb.linearVelocity.normalized * EnemyStats.movement_speed;
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
                TakeDamage(projectile.GetDamage(), collision);
            }

        }
    }

    void TakeDamage(float damageAmount, Collision2D collision)
    {
        EnemyStats.DecreaseHealth(damageAmount);
        floatingHealthBar.UpdateStatusBar(EnemyStats.GetHealth, EnemyStats.maxHealth);     

        Vector2 contactOfdamage = collision.GetContact(0).point;
        Vfx.TakeDamageVFX(contactOfdamage, Quaternion.identity);

        if (EnemyStats.IsDead())
        {
            Vfx.ExplodeVFX(transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}