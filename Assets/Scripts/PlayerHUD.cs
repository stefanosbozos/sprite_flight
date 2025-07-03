using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private UIDocument UIDoc;
    [SerializeField] private RectTransform temperatureBar;
    [SerializeField] private float barWidth, barHeight;
    private ShootingSystem PlayerShootingSystem;
    private Label m_LaserTemperature;
    private VisualElement m_LaserTempMask;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerShootingSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<ShootingSystem>();
        m_LaserTemperature = UIDoc.rootVisualElement.Q<Label>("LaserTemp");
        m_LaserTempMask = UIDoc.rootVisualElement.Q<VisualElement>("LasertempMask");

        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        LaserOverheated();
        LaserTemperatureProgress();
    }

    void LaserOverheated()
    {
        if (PlayerShootingSystem.IsOverheated())
        {
            m_LaserTemperature.text = "OVERHEATED!";
        }
        else
        {
            m_LaserTemperature.text = "Laser Temperature";
        }
    }

    void LaserTemperatureProgress()
    {
        m_LaserTempMask.style.width = Length.Percent(PlayerShootingSystem.GetLaserTemp());

        float newHeight = PlayerShootingSystem.GetLaserTemp() / 100 * barHeight;
        temperatureBar.sizeDelta = new Vector2(barWidth, newHeight);
    }
}
