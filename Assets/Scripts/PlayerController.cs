using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Player thrust force
    [Header("Player movement")]
    [SerializeField] private float thrustForce = 10f;
    [SerializeField] private float maxSpeed = 10f;
    private float brakingForce = 0.5f;
    // Input system
    InputAction moveAction;
    Vector2 moveValue;

    // The gamepad control is different than the keyboard
    InputAction accelerate_gp;
    InputAction brake_gp;


    [Header("Particle Effects")]
    [SerializeField] GameObject boosterFlameSprite;
    [SerializeField] private GameObject explosionParticleEffect;

    Rigidbody2D rb;
    
    private bool isAlive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boosterFlameSprite.SetActive(false);
        rb = GetComponent<Rigidbody2D>();

        //Keyboard support input
        moveAction = InputSystem.actions.FindAction("Move");

        // Gamepad support input
        accelerate_gp = InputSystem.actions.FindAction("Gamepad_Accelerate");
        brake_gp = InputSystem.actions.FindAction("Gamepad_Brake");

        // Player state
        isAlive = true;
    }

    // Update is called once per frame 
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Throttle();
        BoosterFlames();
    }

    void Throttle()
    {
        // Get the direction of the move action and store it to a Vector2
        moveValue = moveAction.ReadValue<Vector2>();

        // Relative force because the "W" is more like throttle
        if (moveValue.y < 0)
        {
            rb.AddRelativeForce(-rb.linearVelocity * brakingForce);
        }
        else
        {
            rb.AddRelativeForce(moveValue * thrustForce);
        }

        // This is to stop the player for accelerating if the move button is constantly pressed.
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // Gamepad support contoller
        if (accelerate_gp.IsPressed())
        {
            moveValue = new Vector2(0, 1);
            rb.AddRelativeForce(moveValue * thrustForce);
        }

        if (brake_gp.IsPressed())
        {
            rb.AddRelativeForce(-rb.linearVelocity * brakingForce);
        }

    }

    void BoosterFlames()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame || accelerate_gp.WasPressedThisFrame())
        {
            boosterFlameSprite.SetActive(true);
        }

        // Stop the thrusters animation if the move is disabled
        if (Keyboard.current.wKey.wasReleasedThisFrame || accelerate_gp.WasReleasedThisFrame())
        {
            boosterFlameSprite.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "asteroid")
        {
            // Instantiate the particle when the player collides with any object in the world
            GameObject explosion = Instantiate(explosionParticleEffect, transform.position, transform.rotation);

            isAlive = false;
            // When the player collides with any other object the player spaceship is destroyed.
            Destroy(gameObject);
            Destroy(explosion, 1f);
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }

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
