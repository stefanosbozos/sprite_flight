using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/*
    This script handles the projectile shooting system of the player.
    It has a cooldown so the shots can be fired every .5f seconds.
*/

public class ShootingSystem : MonoBehaviour
{

    [Header("Projectile")]
    [SerializeField] private Rigidbody2D laserProjectile;
    [SerializeField] private float projectileSpeed = 500.0f;


    [Header("Cooldowns")]
    // The time in seconds between the shots
    [SerializeField] private float timeBetweenShots = 0.05f;
    // Store the time that passed in second since the last shot.
    private float timeSinceLastShot = 0f;
    private bool readyToShoot = true;

    // Input System
    InputAction fire;
    InputAction aim;

    Vector2 lastAimPosition;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fire = InputSystem.actions.FindAction("Fire");
        aim = InputSystem.actions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        Fire();
    }

    void Aim()
    {
        // This is the aiming system for the player
        // Get the mouse screen position, translate that position to world position and put it in a Vector3
        // https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Camera.ScreenToWorldPoint.html
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        //Debug.Log("Mouse Pos:" + mousePos);

        // Calculate the direction to the mouse and normalize it (1)
        //Vector2 direction = (mousePos - transform.position).normalized;
        Vector2 direction = aim.ReadValue<Vector2>();
        Debug.Log("Direction" + direction);

        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Set the Player game object to face this direction - https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Transform-up.html
        //transform.up = direction;

        if (direction.x != 0 && direction.y != 0)
        {
            lastAimPosition = direction;
            Debug.Log("Last: " + lastAimPosition);
            transform.up = lastAimPosition;
        }

    }

    // Need to decide a better name later
    void Fire()
    {
        // This is using the old input system system
        // //increase the time since the last shot
        // timeSinceLastShot += Time.deltaTime;

        // // Check the input and if the player is ready to shoot
        // if (Mouse.current.leftButton.isPressed && readyToShoot)
        // {
        //     // Check whether the elapsed time is greater that the allowed time between the shots
        //     if (timeSinceLastShot >= timeBetweenShots)
        //     {
        //         InstantiateProjectile();
        //         readyToShoot = !readyToShoot;
        //         timeSinceLastShot = 0f;
        //     }
        // }
        // // if 0.5s have elapsed since the last shot, the player is ready to shoot.
        // if (timeSinceLastShot >= timeBetweenShots)
        // {
        //     readyToShoot = !readyToShoot;
        // }

        if (fire.WasPressedThisFrame())
        {
            InstantiateProjectile();
        }
    }

    void InstantiateProjectile()
    {
        Rigidbody2D laser = Instantiate(laserProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation) as Rigidbody2D;

        laser.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileSpeed);

    }
}
