using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Player thrust force
    [Header("Player movement")]
    [SerializeField] private float thrustForce = 100f;
    [SerializeField] private float maxSpeed = 10f;

    [Header("Booster Flames")]
    [SerializeField] GameObject boosterFlameSprite;

    [Header("Particle Effects")]
    [SerializeField] private GameObject explosionParticleEffect;

    Rigidbody2D rb;

    // Input system
    InputAction moveAction;

    Vector2 moveValue;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boosterFlameSprite.SetActive(false);
        // !!! Assign the component to the rb variable which is type of Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame 
    void Update()
    {
        Throttle();
    }

    void Throttle()
    {

        // Get the direction of the move action and store it to a Vector2
        moveValue = moveAction.ReadValue<Vector2>();

        // Move the player
        rb.AddForce(moveValue * thrustForce);

        // This is to stop the player for accelerating if the move button is constantly pressed.
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
        
        
        // Play thrusters animation if the move action is active
        if (moveAction.WasPressedThisFrame())
        {
            boosterFlameSprite.SetActive(true);
        }
        
        // Stop the thrusters animation if the move is disabled
        if (moveAction.WasReleasedThisFrame())
        {
            boosterFlameSprite.SetActive(false);
        }
    }

    // 

    // void BoosterFlames()
    // {
    //     if (Mouse.current.leftButton.wasPressedThisFrame)
    //     {
    //         boosterFlameSprite.SetActive(true);
    //     }
    //     else if (Mouse.current.leftButton.wasReleasedThisFrame)
    //     {
    //         boosterFlameSprite.SetActive(false);
    //     }
    // }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Instantiate the particle when the player collides with any object in the world
    //     Instantiate(explosionParticleEffect, transform.position, transform.rotation);

    //     // When the player collides with any other object the player spaceship is destroyed.
    //     Destroy(gameObject);

    //     // Show the Restart Button when the player dies
    //     restartButton.style.display = DisplayStyle.Flex;

    //     UpdateHighscore();
    // }

}





/*  FURTHER NOTES
    1. The trhustForce is being multiplied by the direction which is the result of subtracting the mousePos from the position of the player object
    the further the mouse click the bigger the result. This causes the speed to vary based on how far the mouse is clicked from the player object in
    the world. This is why we need to follow the technique called "normalization". 

    Normalization keeps direction the same but limits its length to 1.

*/

// Old functionality scripts
//void MoveToDirection()
    // {

    //     // This script moves the player towards where the mouse is pointing.
    //     // Get the mouse screen position, translate that position to world position and put it in a Vector3
    //     // https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Camera.ScreenToWorldPoint.html
    //     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

    //     // Calculate the direction to the mouse and normalize it (1)
    //     Vector2 direction = (mousePos - transform.position).normalized;

    //     // Set the Player game object to face this direction - https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Transform-up.html
    //     transform.up = direction;
        
    //     //Add force towards the direction of the mouse click location(1)
    //     rb.AddForce(direction * thrustForce);

    //     if (rb.linearVelocity.magnitude > maxSpeed)
    //     {
    //         rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    //     }
    // }
