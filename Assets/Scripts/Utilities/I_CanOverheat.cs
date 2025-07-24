public interface I_CanOverheat
{
    public const float k_LaserCooldownInterval = 4f;
    public const float k_LaserHeatLimit = 100f;
    public float GetTemperature();
    public void SetTemperature(float temperature);
    public void DecreaseHeat();
    public void IncreaseHeat();
    public bool IsWithinHeatLimit();
    public bool IsReady();
    public bool IsOverheated();
}