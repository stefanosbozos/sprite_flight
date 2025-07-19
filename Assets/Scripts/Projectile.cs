using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LaserSystemSO m_laserSystem;

    void Update()
    {
        MoveToDirection();
    }

    public void FireProjectileAt(Vector3 spawn_position, Quaternion spawn_rotation)
    {
        Instantiate(gameObject, spawn_position, spawn_rotation);
    }

    public float GetDamage()
    {
        return m_laserSystem.GetLaserDamage();
    }

    void MoveToDirection()
    {
        transform.Translate(m_laserSystem.ProjectileDirection * m_laserSystem.ProjectileSpeed * Time.deltaTime);
        Destroy(gameObject, m_laserSystem.ProjectileTTL);
    }
}