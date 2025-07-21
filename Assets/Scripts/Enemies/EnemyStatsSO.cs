using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    // The type of projectile the enemy has, if non leave null.
    public GameObject ProjectilePreFab;

    // Enemy Vitals
    private float health;
    public float maxHealth = 100f;

    // Movement
    public float movement_speed;
    public float rotation_speed;
    public float damage_rate;

    public void DecreaseHealth(float damageAmount)
    {
        health -= damageAmount;
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public float GetHealth => health;

    public void SetHealth(float healthAmount)
    {
        health = healthAmount;
    }
}