using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    [SerializeField] private float m_health;
    private float m_maxHealth = 100f;
    [SerializeField] private float m_shield;
    private float m_maxShield = 100f;
    private const float k_MaxSpeed = 100f;


    public void DecreaseHealth(float amount)
    {
        m_health -= amount;
    }

    public void DecreaseShield(float amount)
    {
        m_shield -= amount;
    }


    public float Health => m_health;
    public float MaxHealth => m_maxHealth;
    public float Shield => m_shield;
    public float MaxShield => m_maxShield;
    public float MaxSpeed => k_MaxSpeed;

    public void SetHealth(float healthAmount)
    {
        m_health = healthAmount;
    }

    public void SetShield(float shieldAmount)
    {
        m_shield = shieldAmount;
    }
}