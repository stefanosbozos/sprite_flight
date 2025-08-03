using UnityEngine;

public class E_Laser : Projectile
{
    [SerializeField] private float m_damageMultiplier;
    void Update()
    {
        MoveToDirection();
    }

    public override float GetDamage()
    {
        return this.m_damageMultiplier;        
    }
}