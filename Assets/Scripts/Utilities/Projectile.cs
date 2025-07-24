using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected float m_speed;
    protected float m_damage;
    protected const float k_timeToLive = 1.5f;

    public abstract void FireProjectileAt(Vector3 spawn_position, Quaternion spawn_rotation);

    public abstract float GetDamage();

    protected abstract void MoveToDirection();
}