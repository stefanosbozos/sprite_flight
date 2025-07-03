using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/*
    Here is all the functionality of the scoring system and the UI
    elements for the game.
*/

public class UISystem : MonoBehaviour
{
    // The final score
    private float score = 0f;
    // Highscore
    private float highscore = 0f;
    // The multiplier of the Time.deltaTime to create a score for the player
    private float scoreMultiplier = 2.5f;

    [Header("HUD")]
    [SerializeField] private UIDocument uIDocument;
    // Label is a UI Toolkit class that is used to display text
    private Label scoreText;

    private Label highScoreText;

    private Button restartButton;

    private VisualElement heatBar;

    private PlayerController player;

    private ShootingSystem shootingSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // uiDocument gives access to the Document like JS, Q is s querySelector, <> the type of element, "ScoreLabel" the name of the Label we are looking for.
        scoreText = uIDocument.rootVisualElement.Q<Label>("ScoreLabel");

        // Assing the button from the UI document
        restartButton = uIDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;

        // set a listener for the restart button
        restartButton.clicked += ReloadScene;

        heatBar = uIDocument.rootVisualElement.Q<VisualElement>("LasertempMask");

        // Assign the highscore text from the UI Builder
        highScoreText = uIDocument.rootVisualElement.Q<Label>("Highscore");
        highScoreText.style.display = DisplayStyle.None;

        // To be able to check whether the player is alive or not.
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        shootingSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<ShootingSystem>();

        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
    }   

    void Update()
    {
        CheckPlayerStatus();
        UpdateHeatBar();
    }

    public void UpdateScore(int gainedScore)
    {
        // Multiply the time by the score multiplier and round it down to the nearest integer with the Mathf.FloorToInt()
        score += Mathf.FloorToInt(gainedScore * scoreMultiplier);
        scoreText.text = "Score: " + score;
    }

    // Reload the screen after the player clicks on the "Restart" Button
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateHighscore()
    {
        // Check first if there is a key named highscore
        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetFloat("highscore");
            // Check whether the current score is higher than the highscore
            // and if it is a new highscore, delete the previous key from PlayerPrefs
            // and create a new key with the new highscore (peculiarity of PlayerPrefs)
            if (score > highscore)
            {

                highscore = Mathf.CeilToInt(score);
                PlayerPrefs.SetFloat("highscore", 0f);
                PlayerPrefs.SetFloat("highscore", highscore);
            }
        }
        // Simply store the score to the PlayerPrefs
        else
        {
            highscore = Mathf.CeilToInt(score);
            PlayerPrefs.SetFloat("highscore", highscore);
        }
        ShowHighscore();
    }

    void ShowHighscore()
    {
        highScoreText.text = "Highscore: " + highscore;
        highScoreText.style.display = DisplayStyle.Flex;
    }

    void CheckPlayerStatus()
    {
        if (!player.IsAlive())
        {
            UpdateHighscore();
            restartButton.style.display = DisplayStyle.Flex;
            UnityEngine.Cursor.visible = true;
        }
    }

    void UpdateHeatBar()
    { 
        heatBar.style.width = Length.Percent(shootingSystem.GetLaserTemp());
        Debug.Log(Length.Percent(shootingSystem.GetLaserTemp()));
    }
}
