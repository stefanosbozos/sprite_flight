using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Player thrust force
    [Header("Player movement")]
    [SerializeField] private float thrustForce = 1f;
    [SerializeField] private float maxSpeed = 5f;

    [Header("Booster Flames")]
    [SerializeField] GameObject boosterFlameSprite;

    // ++++++++++ Scoring System UI +++++++++++
    // Time elpased until the player dies
    private float elapsedTime = 0f;
    // The final score
    private float score = 0f;
    // The multiplier of the Time.deltaTime to create a score for the player
    private float scoreMultiplier = 10f;

    [Header("HUD")]
    [SerializeField] private UIDocument uIDocument;
    // Label is a UI Toolkit class that is used to display text
    private Label scoreText;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boosterFlameSprite.SetActive(false);
        // !!! Assign the component to the rb variable which is type of Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // uiDocument gives access to the Document like JS, Q is s querySelector, <> the type of element, "ScoreLabel" the name of the Label we are looking for.
        scoreText = uIDocument.rootVisualElement.Q<Label>("ScoreLabel");
    }

    // Update is called once per frame 
    void Update()
    {
        UserInput();
        BoosterFlames();
        UpdateScore();
    }

    void UserInput()
    {
        /*
            Gets the mouse click on the screen, translates it to world position and 
            moves the player spaceship towards that direction.
        */

        if (Mouse.current.leftButton.isPressed)
        {

            // Get the mouse screen position, translate that position to world position and put it in a Vector3
            // https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Camera.ScreenToWorldPoint.html
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

            // Calculate the direction to the mouse and normalize it (1)
            Vector2 direction = (mousePos - transform.position).normalized;

            // Set the Player game object to face this direction - https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Transform-up.html
            transform.up = direction;

            // Add force towards the direction of the mouse click location(1)
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
    }

    void BoosterFlames()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            boosterFlameSprite.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            boosterFlameSprite.SetActive(false);
        }
    }

    void UpdateScore()
    {
        // Time.deltaTime is the time in seconds since the last frame. - https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Time-deltaTime.html
        elapsedTime += Time.deltaTime;

        // Multiply the time by the score multiplier and round it down to the nearest integer with the Mathf.FloorToInt()
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // When the player collides with any other object the player spaceship is destroyed.
        Destroy(gameObject);
    }

}



/*  FURTHER NOTES
    1. The trhustForce is being multiplied by the direction which is the result of subtracting the mousePos from the position of the player object
    the further the mouse click the bigger the result. This causes the speed to vary based on how far the mouse is clicked from the player object in
    the world. This is why we need to follow the technique called "normalization". 

    Normalization keeps direction the same but limits its length to 1.

*/
