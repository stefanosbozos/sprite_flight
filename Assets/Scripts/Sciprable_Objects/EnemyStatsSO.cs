using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    // The type of projectile the enemy has, if non leave null.
    public GameObject ProjectilePreFab;

    public int m_scoreValue;

    // Movement
    public float movement_speed;
    public float rotation_speed;
    public float damage_rate;

    public float minDistanceFromOtherEnemies = 5f;
    public float maxDistanceFromOtherEnemies = 20;

}