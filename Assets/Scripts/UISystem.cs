using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/*
    Here is all the functionality of the scoring system and the UI
    elements for the game.
*/

public class UISystem : MonoBehaviour
{
    // Time elpased until the player dies
    private float elapsedTime = 0f;
    // The final score
    private float score = 0f;
    // Highscore
    private float highscore = 0f;
    // The multiplier of the Time.deltaTime to create a score for the player
    private float scoreMultiplier = 10f;

    [Header("HUD")]
    [SerializeField] private UIDocument uIDocument;
    // Label is a UI Toolkit class that is used to display text
    private Label scoreText;

    private Label highScoreText;

    private Button restartButton;

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

        // Assign the highscore text from the UI Builder
        highScoreText = uIDocument.rootVisualElement.Q<Label>("Highscore");
        highScoreText.style.display = DisplayStyle.None;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        // Time.deltaTime is the time in seconds since the last frame. - https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Time-deltaTime.html
        elapsedTime += Time.deltaTime;

        // Multiply the time by the score multiplier and round it down to the nearest integer with the Mathf.FloorToInt()
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
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
}
