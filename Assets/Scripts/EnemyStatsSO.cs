using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    // Enemy Vitals
    public float maxHealth = 100f;

    // Movement
    public float movement_speed;
    public float rotation_speed;
    public float damage_rate;

    // VFX
    public GameObject taking_damage_VFX;
    public GameObject death_explosion_VFX;
}