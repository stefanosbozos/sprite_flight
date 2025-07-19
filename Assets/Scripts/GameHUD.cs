using UnityEngine;
using UnityEngine.UIElements;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private UIDocument m_UIDoc;
    [SerializeField] private PlayerStatsSO m_playerStats;

    private VisualElement m_healthBar;
    private Label m_healthPercentage;
    private VisualElement m_shieldBar;
    private Label m_shieldPercentage;
    private VisualElement m_laserBar;
    private Label m_laserPercentage;
    private Label m_resources;

    void Awake()
    {
        m_healthBar = m_UIDoc.rootVisualElement.Q<VisualElement>("HealthBarFill");
        m_shieldBar = m_UIDoc.rootVisualElement.Q<VisualElement>("ShieldBarFill");
        m_laserBar = m_UIDoc.rootVisualElement.Q<VisualElement>("LaserBarFill");

        m_healthPercentage = m_UIDoc.rootVisualElement.Q<Label>("HealthBarPercentage");
        m_shieldPercentage = m_UIDoc.rootVisualElement.Q<Label>("ShieldBarPercentage");
        m_laserPercentage = m_UIDoc.rootVisualElement.Q<Label>("LaserBarPercentage");
        m_resources = m_UIDoc.rootVisualElement.Q<Label>("Resources");
    }

    void Update()
    {
        UpdateHealthBar();
        UpdateShieldBar();
        UpdateLaserBar();
        UpdateResources();
    }

    void UpdateHealthBar()
    {
        m_healthBar.style.width = Length.Percent(Mathf.FloorToInt(m_playerStats.Health));
        m_healthPercentage.text = m_playerStats.Health + "/" + m_playerStats.MaxHealth;
    }

    void UpdateShieldBar()
    {
        m_shieldBar.style.width = Length.Percent(Mathf.FloorToInt(m_playerStats.Shield));
        m_shieldPercentage.text = m_playerStats.Shield + "/" + m_playerStats.MaxShield;
    }

    void UpdateLaserBar()
    {
        m_laserBar.style.width = Mathf.FloorToInt(m_playerStats.LaserSystem.LaserTemperature);
        m_laserPercentage.text = m_playerStats.LaserSystem.LaserTemperature + "/" + m_playerStats.LaserSystem.laserHeatLimit;
    }

    void UpdateResources()
    {
        m_resources.text = "Resources: ";
    }
}
