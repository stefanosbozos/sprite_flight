using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    // Movement
    public float ThrustForce;
    public float RotationSpeed;
    public const float k_MaxSpeed = 20;

    // Player Vitals
    public const float k_MaxHealth = 100;
    public const float k_MaxShield = 100;

    // Player VFX
    public GameObject DeathFX;
    public GameObject TakeDamageFX;
}