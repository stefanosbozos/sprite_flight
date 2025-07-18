using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;
    [SerializeField] private LaserSystemSO m_laserSystem;
    [SerializeField] private Transform m_bulletSpawnPosition;
    [SerializeField] private float m_timeBetweenShots;
    [SerializeField] private float m_laserCooldownInterval = 5f;

    private InputAction m_shootLaser;
    private float m_timeSinceLastShot;
    private float m_laserTemperature;


    void Awake()
    {
        m_shootLaser = InputSystem.actions.FindAction("Shoot");
    }

    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        // Keep track of the time that the lasers are not fired.
        m_timeSinceLastShot += Time.deltaTime;

        if (m_shootLaser.inProgress)
        {

            if (m_timeSinceLastShot >= m_timeBetweenShots)
            {

                if (m_laserTemperature < m_laserSystem.laserHeatLimit)
                {
                    FireLaser();
                }
                m_timeSinceLastShot = 0f;

            }

        }

        CheckGunsTemperature();
    }

    private void CheckGunsTemperature()
    {
        if (m_laserTemperature >= 0 && m_laserTemperature < m_laserSystem.laserHeatLimit)
        {
            m_laserTemperature -= Time.deltaTime * m_laserSystem.LaserCooldownDecreatingStep;
        }

        if (m_laserTemperature >= m_laserSystem.laserHeatLimit)
        {
            CooldownLaser();
        }
    }

    private void CooldownLaser()
    {
        m_laserCooldownInterval -= Time.deltaTime;

        if (m_laserTemperature >= m_laserSystem.laserHeatLimit && m_laserCooldownInterval <= 0.0f)
        {
            m_laserTemperature = 0f;
            m_laserCooldownInterval = 3f;
        }

    }

    private void FireLaser()
    {
        m_laserSystem.projectile.FireProjectileAt(m_bulletSpawnPosition.position, m_bulletSpawnPosition.rotation);
        m_laserTemperature += m_laserSystem.LaserHeatIncreaseStep;
    }
    
    public float GunsTemperature => m_laserTemperature;
}