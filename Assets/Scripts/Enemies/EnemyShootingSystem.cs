using UnityEngine;

public class EnemyShootingSystem : MonoBehaviour
{
    public EnemyStatsSO EnemyStats;

    [SerializeField] private float minTimeToAttack = 1.5f;
    [SerializeField] private float maxTimeToAttack = 5.0f;
    [SerializeField] private float timeBetweenShots;
    private float timer = 0f;
    private Projectile m_projectile;

    void Awake()
    {
        m_projectile = EnemyStats.ProjectilePreFab.GetComponent<Projectile>();
    }

    void Start()
    {
        timeBetweenShots = Random.Range(minTimeToAttack, maxTimeToAttack);
    }

    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        timer += Time.deltaTime;
        if (IsReadyToShoot())
        {
            m_projectile.FireProjectileAt(transform.Find("ProjectileSpawn").position, transform.Find("ProjectileSpawn").rotation);
            timer = 0;
        }
    }

    bool IsReadyToShoot()
    {
        return timer > timeBetweenShots;
    }
}