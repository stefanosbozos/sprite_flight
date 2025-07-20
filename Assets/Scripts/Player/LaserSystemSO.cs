using UnityEngine;

[CreateAssetMenu(fileName = "LaserSystem", menuName = "Laser System")]
public class LaserSystemSO : ScriptableObject
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Vector3 m_projectileDirection;
    [SerializeField] private float m_projectileSpeed;

    [SerializeField] private float m_damageAmount;
    [SerializeField] private int m_damageMultiplier;

    [SerializeField] private float m_laserHeatDecreaseStep;
    [SerializeField] private float m_laserHeatIncreaseStep;
    public float m_laserTemperature;

    [SerializeField] private float m_timeBetweenShots;
    private float m_cooldownTimer;
    public const float k_LaserCooldownInterval = 4f;
    private const float k_LaserHeatLimit = 100f;
    private const float k_TimeToLive = 1.5f;

    // Methods
    public Vector3 LaserMovement()
    {
        return m_projectileDirection * m_projectileSpeed;
    }

    public float GetLaserDamage()
    {
        return m_damageAmount * m_damageMultiplier;
    }

    public bool IsLaserReady(float timeSincLastShot)
    {
        return timeSincLastShot >= m_timeBetweenShots;
    }

    public bool IsLaserWithinHeatLimit()
    {
        return m_laserTemperature >= 0 && m_laserTemperature < k_LaserHeatLimit;
    }

    public bool IsLaserCool()
    {
        return m_laserTemperature < k_LaserHeatLimit;
    }

    // Getters
    public Projectile GetProjectile => projectile;
    public Vector3 ProjectileDirection => m_projectileDirection;
    public float ProjectileSpeed => m_projectileSpeed;
    public float LaserTemperature => m_laserTemperature;
    public float HeatDecreaseStep => m_laserHeatDecreaseStep;
    public float HeatIncreaseStep => m_laserHeatIncreaseStep;
    public float LaserCooldownInterval => k_LaserCooldownInterval;
    public float laserHeatLimit => k_LaserHeatLimit;
    public float ProjectileTTL => k_TimeToLive;

    public float CooldownTimer()
    {
        return (float)System.Math.Round(m_cooldownTimer, 2);
    }

    public void UpdateCooldownTimer(float time)
    {
        m_cooldownTimer = time;
    }
    
}