using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Player thrust force
    [Header("Player movement")]
    [SerializeField] private float thrustForce = 10f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float maxSpeed = 10f;

    // Input system
    // Move Up Down and Rotate left/right
    private InputAction movePlayer;
    [SerializeField] private Vector2 moveValue;

    private InputAction aim;
    [SerializeField] private Vector2 aimValue;

    // These are the variables for the mouse cursor
    // [Header("Player Cursor")]
    // [SerializeField] private Texture2D cursorTexture;
    // private CursorMode cursorMode = CursorMode.Auto;
    // private Vector2 cursorHotSpot = Vector2.zero;


    [Header("Player Vitals")]
    [SerializeField] private int heatlh;
    [SerializeField] private int shield;
    private bool shieldActive;

    // Player's firing system
    [Header("Firing System")]
    [SerializeField] private GameObject projectilePreFab;
    [SerializeField] private GameObject gunMuzzles;
    InputAction shootLaser;
    private float HEAT_LIMIT = 100f;
    private float laserTemperature = 0f;
    [SerializeField] float timeBetweenShots = 0.01f;
    private float timeSinceLastShot = 0f;
    [SerializeField] private float laserCooldownInterval = 5f;
    private float laserCooldownDecreatingStep = 3f;
    [SerializeField] private float laserHeatIncreaseStep = 3f;
    private int rotationOffset = 90;
    private float aimSensitivity = 10.0f;


    [Header("Player Visual FX")]
    [SerializeField] private ParticleSystem playerVFX;
    [SerializeField] private GameObject explosionParticleEffect;
    [SerializeField] private GameObject onImpactExplosion;
    [SerializeField] private ParticleSystem shipSmoke;

    // Pause System
    private PauseSystem pauseSystem;

    int rotationZ;

    Rigidbody2D rb;

    void Awake()
    {
        pauseSystem = GameObject.FindGameObjectWithTag("game_manager").GetComponent<PauseSystem>();
        //Cursor.SetCursor(cursorTexture, cursorHotSpot, cursorMode);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gunMuzzles.SetActive(false);

        rb = GetComponent<Rigidbody2D>();

        //Keyboard support input
        movePlayer = InputSystem.actions.FindAction("Move");
        shootLaser = InputSystem.actions.FindAction("Shoot");
        aim = InputSystem.actions.FindAction("Aim");

        // Player state
        heatlh = 100;
        shield = 0;
    }

    // Update is called once per frame 
    void Update()
    {
        if (!pauseSystem.GameIsPaused())
        {
            PlayerMovement();
            playerVisualEffects();
            ShootProjectile();
            Aim();
        }
    }

    void PlayerMovement()
    {
        moveValue = movePlayer.ReadValue<Vector2>().normalized;
        
        //transform.localRotation = Quaternion.Euler(0f, 0f, -90 * moveValue.x);
        
        rb.AddForce(moveValue * thrustForce);

        //PlayerDirection();
        
        // This is to stop the player for accelerating if the move button is constantly pressed.
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }

    }


    // void PlayerDirection()
    // {
    //     float playerFacingDirection = 0;

    //     if (rb.linearVelocityX > 0)
    //     {
    //         playerFacingDirection = 270f;
    //     }
    //     else if (rb.linearVelocityX < 0)
    //     {
    //         playerFacingDirection = 90f;
    //     }

    //     if (rb.linearVelocityY > 0)
    //     {
    //         playerFacingDirection = 0f;
    //     }
    //     else if (rb.linearVelocityY < 0)
    //     {
    //         playerFacingDirection = 180f;
    //     }

    //     transform.localRotation = Quaternion.Euler(0f, 0f, playerFacingDirection);
    // }

    void Aim()
    {
        Vector3 mouseScreenPos = aim.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));

        Vector3 playerDirection = mouseWorldPos - transform.position;
        playerDirection.z = 0; // Ensure that the player does not rotate on the Z-axis

        if (playerDirection.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - rotationOffset);
        }
    }

    void ShootProjectile()
    {
        // Keep track of the time that the lasers are not fired.
        timeSinceLastShot += Time.deltaTime;

        if (shootLaser.inProgress)
        {
            if (timeSinceLastShot >= timeBetweenShots)
            {
                if (laserTemperature < HEAT_LIMIT)
                {
                    Instantiate(projectilePreFab, transform.Find("LaserSpawn").position, transform.Find("LaserSpawn").rotation);
                    laserTemperature += laserHeatIncreaseStep;
                }
                timeSinceLastShot = 0f;
            }
        }
        CheckGunsTemperature();
    }

    void CheckGunsTemperature()
    {
        if (laserTemperature >= 0 && laserTemperature < HEAT_LIMIT)
        {
            laserTemperature -= Time.deltaTime * laserCooldownDecreatingStep;
        }

        if (laserTemperature >= HEAT_LIMIT)
        {
            CooldownLaser();
        }
    }

    void CooldownLaser()
    {
        laserCooldownInterval -= Time.deltaTime;
        if (laserTemperature >= HEAT_LIMIT && laserCooldownInterval <= 0.0f)
        {
            laserTemperature = 0f;
            laserCooldownInterval = 3f;
        }
    }

    void playerVisualEffects()
    {

        if (moveValue.y > 0)
        {
            playerVFX.emissionRate = 30;
        }
        else
        {
            playerVFX.emissionRate = 0;
        }

        if (shootLaser.inProgress && laserTemperature < HEAT_LIMIT)
        {
            gunMuzzles.SetActive(true);
        }
        else
        {
            gunMuzzles.SetActive(false);
        }

        // Play smoke if player below 20% health
        if (heatlh <= 20)
        {
            shipSmoke.Play();
        }
        else
        {
            shipSmoke.Stop();
            shipSmoke.Clear();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy_fire")
        {
            // Instantiate an explosion where the player got hit
            Vector2 contactOfImpact = collision.GetContact(0).point;
            GameObject impactFX = Instantiate(onImpactExplosion, contactOfImpact, Quaternion.identity);
            if (shieldActive)
            {
                shield -= 10;
            }
            else
            {
                heatlh -= 10;
            }
            Destroy(collision.gameObject);
            Destroy(impactFX, 1f);

            if (!PlayerAlive())
            {
                KillPlayer();
            }

        }
    }

    
    void ActivateShields()
    {
        shieldActive = !shieldActive;
        shield = 100;
    }

    void KillPlayer()
    {
        GameObject OnDeathExplosion = Instantiate(explosionParticleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(OnDeathExplosion, 3f);
    }
    public bool PlayerAlive()
    {
        if (heatlh > 0)
        {
            return true;
        }
        return false;
    }
    public float GetGunsTemperature()
    {
        return laserTemperature;
    }
    public bool GunsOverheated()
    {
        return laserTemperature >= HEAT_LIMIT ? true : false;
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
