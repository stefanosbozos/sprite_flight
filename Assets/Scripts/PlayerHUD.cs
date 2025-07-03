using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private UIDocument UIDoc;

    // This is for gun temperature bar next to the player
    [SerializeField] private RectTransform temperatureBar;
    [SerializeField] private float barWidth, barHeight;

    // The player's stuff
    private GameObject Player;
    private ShootingSystem PlayerShootingSystem;
    private Label m_LaserTemperature;
    private VisualElement m_LaserTempMask;

    // The HUD items
    private Label HUD_score;
    private VisualElement HUD_LaserTempBar;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerShootingSystem = Player.GetComponent<ShootingSystem>();

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
