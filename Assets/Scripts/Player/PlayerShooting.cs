using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;
    [SerializeField] private P_Laser laser;
    [SerializeField] private Transform m_bulletSpawnPosition;

    private InputAction m_shootLaser;
    private float m_timeSinceLastShot;
    [SerializeField] private float m_timeBetweenShoots = 0.5f;
    private float m_laserCooldDownTimer;


    void Awake()
    {
        m_shootLaser = InputSystem.actions.FindAction("Shoot");
    }

    void Start()
    {
        m_laserCooldDownTimer = I_CanOverheat.k_LaserCooldownInterval;
        laser.SetTemperature(0);
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

            if (m_timeSinceLastShot > m_timeBetweenShoots)
            {

                if (laser.IsReady())
                {
                    Fire();
                }
                m_timeSinceLastShot = 0f;

            }

        }

        CheckGunsTemperature();
    }

    private void CheckGunsTemperature()
    {
        if (laser.IsWithinHeatLimit())
        {
            laser.DecreaseHeat();
        }
        else
        {
            CooldownLaser();
        }
    }

    private void CooldownLaser()
    {
        m_laserCooldDownTimer -= Time.smoothDeltaTime;
        if (laser.IsOverheated() && m_laserCooldDownTimer <= 0.0f)
        {
            laser.SetTemperature(0);
            m_laserCooldDownTimer = I_CanOverheat.k_LaserCooldownInterval;
        }

    }

    private void Fire()
    {
        laser.FireProjectileAt(m_bulletSpawnPosition.position, m_bulletSpawnPosition.rotation);
        laser.IncreaseHeat();
    }

    public float CooldownTimer => m_laserCooldDownTimer;
}