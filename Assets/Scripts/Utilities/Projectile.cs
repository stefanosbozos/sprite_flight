using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float m_speed;
    protected float m_damage;
    protected const float k_timeToLive = 1.5f;

    public abstract float GetDamage();
    
    public void FireProjectileAt(Vector3 spawn_position, Quaternion spawn_rotation)
    {
        Instantiate(gameObject, spawn_position, spawn_rotation);
    }

    protected void MoveToDirection()
    {
        transform.Translate(Vector3.up * m_speed * Time.deltaTime);
        Destroy(gameObject, k_timeToLive);
    }
}