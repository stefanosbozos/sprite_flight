using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;
    [SerializeField] private LaserSystemSO m_laserSystem;
    [SerializeField] private Transform m_bulletSpawnPosition;

    private InputAction m_shootLaser;
    private float m_timeSinceLastShot;

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

            if (m_laserSystem.IsLaserReady(m_timeSinceLastShot))
            {

                if (m_laserSystem.IsLaserCool())
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
        if (m_laserSystem.IsLaserWithinHeatLimit())
        {
            m_laserSystem.DecreaseLaserTemperature(Time.deltaTime);
        }
        else
        {
            m_laserSystem.CooldownLaser(Time.deltaTime);
        }
    }

    // private void CooldownLaser()
    // {
    //     m_laserCooldownInterval -= Time.deltaTime;

    //     if (m_laserTemperature >= m_laserSystem.laserHeatLimit && m_laserCooldownInterval <= 0.0f)
    //     {
    //         m_laserTemperature = 0f;
    //         m_laserCooldownInterval = 3f;
    //     }

    // }

    private void FireLaser()
    {
        m_laserSystem.GetProjectile.FireProjectileAt(m_bulletSpawnPosition.position, m_bulletSpawnPosition.rotation);
        m_laserSystem.IncreaseLaserTemperature();
    }
}