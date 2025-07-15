using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private WeaponSO m_weapon;

    private float timeToLive = 1.5f;

    void Update()
    {
        MoveToTarget();
    }

    public void FireProjectileAt(Vector3 spawn_position, Quaternion spawn_rotation)
    {
        Instantiate(gameObject, spawn_position, spawn_rotation);
    }

    void MoveToTarget()
    {
        transform.Translate(m_weapon.projectile_direction * m_weapon.projectile_speed * Time.deltaTime);
        Destroy(gameObject, timeToLive);
    }

    public float GetDamageAmount()
    {
        return m_weapon.damage_amount;
    }

}