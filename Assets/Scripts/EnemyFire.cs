using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] WeaponSO weapon;

    private float timeToLive = 1.5f;

    void Update()
    {
        transform.Translate(weapon.projectile_direction * weapon.projectile_speed * Time.deltaTime);  
        Destroy(gameObject, timeToLive);
    }
}