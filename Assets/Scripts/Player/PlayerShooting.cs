using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;
    [SerializeField] private LaserSystemSO m_laserSystem;
    [SerializeField] private Transform m_bulletSpawnPosition;

    private InputAction m_shootLaser;
    private float m_timeSinceLastShot;
    private float m_laserCooldDownTimer;


    void Awake()
    {
        m_shootLaser = InputSystem.actions.FindAction("Shoot");
    }

    void Start()
    {
        m_laserCooldDownTimer = m_laserSystem.LaserCooldownInterval;
        m_laserSystem.m_laserTemperature = 0;
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
            m_laserSystem.m_laserTemperature -= m_laserSystem.HeatDecreaseStep * Time.deltaTime;
        }
        else
        {
            CooldownLaser();
        }
    }

    private void CooldownLaser()
    {
        m_laserCooldDownTimer -= Time.smoothDeltaTime;
        m_laserSystem.UpdateCooldownTimer(m_laserCooldDownTimer);

        if (m_laserSystem.m_laserTemperature >= m_laserSystem.laserHeatLimit && m_laserCooldDownTimer <= 0.0f)
        {
            m_laserSystem.m_laserTemperature = 0f;
            m_laserCooldDownTimer = m_laserSystem.LaserCooldownInterval;
        }

    }

    private void FireLaser()
    {
        m_laserSystem.GetProjectile.FireProjectileAt(m_bulletSpawnPosition.position, m_bulletSpawnPosition.rotation);
        m_laserSystem.m_laserTemperature += m_laserSystem.HeatIncreaseStep;
    }

    public float CooldownTimer => m_laserCooldDownTimer;
}