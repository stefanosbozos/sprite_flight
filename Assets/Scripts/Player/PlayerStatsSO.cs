using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    public ScriptableObject config;

    // Player Movement
    [SerializeField] private float m_thrustForce;
    private const float k_MaxSpeed = 100f;
    [SerializeField] private float m_rotationSpeed;

    // Player Vitals
    private const float k_MaxHealth = 100f;

    // Player VFX
    [SerializeField] private GameObject m_takeDamageVFX;
    [SerializeField] private GameObject m_explosionVFX;


    // Getters
    public float ThrustForce => m_thrustForce;
    public float RotationSpeed => m_rotationSpeed;
    public float MaxHealth => k_MaxHealth;
    public float MaxSpeed => k_MaxSpeed;
    public GameObject TakeDamageFX => m_takeDamageVFX;
    public GameObject ExplodeFX => m_explosionVFX;

}