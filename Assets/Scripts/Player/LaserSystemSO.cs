using UnityEngine;

[CreateAssetMenu(fileName = "LaserSystem", menuName = "Laser System")]
public class LaserSystemSO : ScriptableObject
{
    public float LaserCooldownDecreatingStep;
    public float LaserHeatIncreaseStep;
    public float laserHeatLimit;
    public Projectile projectile;
}