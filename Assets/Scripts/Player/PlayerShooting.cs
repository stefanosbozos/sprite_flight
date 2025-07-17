using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public PlayerStatsSO PlayerStats;
    public LaserSystemSO LaserSystem;
    public Transform BulletSpawnPosition;
    public float TimeBetweenShots;
    public float LaserCooldownInterval = 5f;

    private InputAction m_shootLaser;
    private float m_timeSinceLastShot;
    private float m_laserTemperature;

    private PlayerVFX m_playerVFX;


    void Awake()
    {
        m_shootLaser = InputSystem.actions.FindAction("Shoot");
        m_playerVFX = GetComponent<PlayerVFX>();
    }

    public void Shoot()
    {
        // Keep track of the time that the lasers are not fired.
        m_timeSinceLastShot += Time.deltaTime;

        if (m_shootLaser.inProgress)
        {

            if (m_timeSinceLastShot >= TimeBetweenShots)
            {

                if (m_laserTemperature < LaserSystem.laserHeatLimit)
                {
                    m_playerVFX.GunMuzzleFlash(true);
                    FireLaser();
                }
                m_timeSinceLastShot = 0f;
                
            }

        }

        m_playerVFX.GunMuzzleFlash(false);
        CheckGunsTemperature();
    }

    void CheckGunsTemperature()
    {
        if (m_laserTemperature >= 0 && m_laserTemperature < LaserSystem.laserHeatLimit)
        {
            m_laserTemperature -= Time.deltaTime * LaserSystem.LaserCooldownDecreatingStep;
        }

        if (m_laserTemperature >= LaserSystem.laserHeatLimit)
        {
            CooldownLaser();
        }
    }

    void CooldownLaser()
    {
        LaserCooldownInterval -= Time.deltaTime;

        if (m_laserTemperature >= LaserSystem.laserHeatLimit && LaserCooldownInterval <= 0.0f)
        {
            m_laserTemperature = 0f;
            LaserCooldownInterval = 3f;
        }

    }

    void FireLaser()
    {
        LaserSystem.projectile.FireProjectileAt(BulletSpawnPosition.position, BulletSpawnPosition.rotation);
        m_laserTemperature += LaserSystem.LaserHeatIncreaseStep;
    }
    
    public float GunsTemperature => m_laserTemperature;
}