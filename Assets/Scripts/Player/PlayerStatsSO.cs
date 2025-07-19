using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    // Player Movement
    [SerializeField] private float m_thrustForce;
    [SerializeField] private float m_rotationSpeed;
    private const float k_MaxSpeed = 100f;

    // Player Vitals
    private float m_health;
    private float m_shield;
    private float m_maxHealth = 100;
    private float m_maxShield = 50;

    // Shooting System
    [SerializeField] private LaserSystemSO m_laserSystem;

    // Member Methods
    public bool InCriticalState()
    {
        return m_health <= 20;
    }

    public bool IsDead()
    {
        return m_health <= 0;
    }

    public void DamageToHealth(float damageAmount)
    {
        m_health -= damageAmount;
    }

    public void DamageToShield(float damageAmount)
    {
        m_shield -= damageAmount;
    }

    public bool ShieldActive()
    {
        return m_shield > 0;
    }

    // Getters
    public float ThrustForce => m_thrustForce;
    public float RotationSpeed => m_rotationSpeed;
    public float Health => m_health;
    public float Shield => m_shield;
    public float MaxHealth => m_maxHealth;
    public float MaxShield => m_maxShield;
    public float MaxSpeed => k_MaxSpeed;
    public LaserSystemSO LaserSystem => m_laserSystem;


    // Setters
    public void SetCurrentHealth(float healthValue)
    {
        m_health = healthValue;
    }

    public void SetCurrentShield(float shieldValue)
    {
        m_shield = shieldValue;
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        m_maxHealth = newMaxHealth;
    }

    public void SetMaxShield(float newMaxShield)
    {
        m_maxHealth = newMaxShield;
    }

}