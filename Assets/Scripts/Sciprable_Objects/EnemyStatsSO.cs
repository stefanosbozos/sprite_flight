using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    // Movement
    public float movement_speed;
    public float rotation_speed;
    public float damage_rate;

    public float minDistanceFromOtherEnemies = 5f;
    public float maxDistanceFromOtherEnemies = 20;

}