using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    private UIDocument UIDoc;
    

    // // This is for gun temperature bar next to the player
    // [SerializeField] private RectTransform temperatureBar;
    // [SerializeField] private float barWidth, barHeight;

    // The player's stuff
    private Player player;

    // The HUD items
    private Label HUD_score;

    // The laser progress bar
    private VisualElement m_LaserTempMask;
    private Label m_LaserTemperature;


    // Colors of the UI elements
    Color standard_lightBlue = new Color32(22, 224, 255, 255);
    Color warning_orange = new Color32(255, 196, 2, 255);
    //Color warning_red = new Color32(255, 2, 2, 255);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        m_LaserTemperature = UIDoc.rootVisualElement.Q<Label>("HUD_LaserTemp");
        m_LaserTempMask = UIDoc.rootVisualElement.Q<VisualElement>("HUD_LasertempMask");

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
        if (player.GunsOverheated())
        {
            m_LaserTemperature.style.color = new StyleColor(warning_orange);
            m_LaserTemperature.text = "OVERHEATED!";
        }
        else
        {
            m_LaserTemperature.style.color = new StyleColor(standard_lightBlue);
            m_LaserTemperature.text = "Laser Temperature";
        }
    }

    void LaserTemperatureProgress()
    {
        m_LaserTempMask.style.width = Length.Percent(player.GetGunsTemperature());
    }
}
