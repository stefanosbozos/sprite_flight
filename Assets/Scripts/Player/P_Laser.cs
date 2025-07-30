using UnityEngine;

public class P_Laser : Projectile, I_CanOverheat
{
    [SerializeField] private float m_laserHeatDecreaseStep;
    [SerializeField] private float m_laserHeatIncreaseStep;
    private float m_laserTemperature;


    void Start()
    {
        m_damage = 10;
    }

    void Update()
    {
        MoveToDirection();
    }

    public override float GetDamage()
    {
        return m_damage;
    }

    public void DecreaseHeat()
    {
        m_laserTemperature -= m_laserHeatDecreaseStep * Time.deltaTime;
    }

    public void IncreaseHeat()
    {
        m_laserTemperature += m_laserHeatIncreaseStep;
        Debug.Log(m_laserTemperature);
    }

    public bool IsOverheated()
    {
        return m_laserTemperature >= I_CanOverheat.k_LaserHeatLimit;
    }

    public bool IsWithinHeatLimit()
    {
        return m_laserTemperature >= 0 && m_laserTemperature < I_CanOverheat.k_LaserHeatLimit;
    }

    public bool IsReady()
    {
        return m_laserTemperature < I_CanOverheat.k_LaserHeatLimit;

    }

    public float GetTemperature()
    {
        return m_laserTemperature;
    }

    public void SetTemperature(float temperature)
    {
        m_laserTemperature = temperature;
    }


}