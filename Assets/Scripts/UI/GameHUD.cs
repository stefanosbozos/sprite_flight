using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private UIDocument m_UIDoc;
    [SerializeField] private PlayerStatsSO m_playerStats;

    private int m_score;
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
        m_resources = m_UIDoc.rootVisualElement.Q<Label>("Score");
    }

    void Start()
    {
        UpdateScore(0);
    }

    void Update()
    {
        UpdateHealthBar();
        UpdateShieldBar();
        UpdateLaserBar();
    }

    void UpdateHealthBar()
    {
        int healthValue = Mathf.Clamp(Mathf.FloorToInt(m_playerStats.Health), 0, m_playerStats.MaxHealth);
        m_healthBar.style.width = Length.Percent(healthValue);
        m_healthPercentage.text = healthValue + "/" + m_playerStats.MaxHealth;
    }

    void UpdateShieldBar()
    {
        int shieldValue = Mathf.Clamp(Mathf.FloorToInt(m_playerStats.Shield), 0, m_playerStats.MaxShield);
        m_shieldBar.style.width = Length.Percent(shieldValue);
        m_shieldPercentage.text = shieldValue + "/" + m_playerStats.MaxShield;
    }

    void UpdateLaserBar()
    {
        int laserValue = Mathf.Clamp(Mathf.FloorToInt(m_playerStats.LaserSystem.LaserTemperature), 0, 100);
        m_laserBar.style.width = Length.Percent(laserValue);
        m_laserPercentage.text = laserValue + "/" + m_playerStats.LaserSystem.laserHeatLimit;

        if (laserValue >= 100)
        {
            m_laserPercentage.text = m_playerStats.LaserSystem.CooldownTimer() + "s";
        }
    }

    public void UpdateScore(int scoreValue)
    {
        m_score += scoreValue;
        m_resources.text = "Resources: " + m_score;
    }
}
