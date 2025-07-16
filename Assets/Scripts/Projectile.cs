using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_damageAmount;
    [SerializeField] private Vector3 m_rojectileDirection;
    [SerializeField] private float m_projectileSpeed;

    private float m_damageMultiplier;
    private float timeToLive = 1.5f;


    public void FireProjectileAt(Vector3 spawn_position, Quaternion spawn_rotation)
    {
        Instantiate(gameObject, spawn_position, spawn_rotation);
    }


    void MoveToDirection()
    {
        transform.Translate(m_rojectileDirection * m_projectileSpeed * Time.deltaTime);
        Destroy(gameObject, timeToLive);
    }


    public float GetDamageAmount => m_damageAmount;

    public void SetDamageMultiplier(float multiplierValue) => m_damageMultiplier = multiplierValue;
    
    
    void Update()
    {
        MoveToDirection();
    }
}