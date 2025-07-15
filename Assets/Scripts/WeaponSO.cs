using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Weapon Stats")]
public class WeaponSO : ScriptableObject
{
    public GameObject projectile_prefab;
    public float damage_amount;
    public Vector3 projectile_direction;
    public float projectile_speed;

}
