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

    //[Header("Cooldowns")]
    // The time in seconds between the shots
    //[SerializeField] private float timeBetweenShots = 0.05f;
    // Store the time that passed in second since the last shot.
    //private float timeSinceLastShot = 0f;
    //private bool readyToShoot = true;

    // Input System
    Camera mainCam;
    Vector3 mousePos;

    InputAction shoot;
    InputAction aim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
        shoot = InputSystem.actions.FindAction("Shoot");
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        Shoot();
    }

    void Aim()
    {
        mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.value);

        Vector3 playerRotation = mousePos - transform.position;

        float rotationZ = Mathf.Atan2(playerRotation.y, playerRotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        
    }

    void Shoot()
    {
        if (shoot.WasPressedThisFrame())
        {
            InstantiateProjectile();
        }

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
    }

    void InstantiateProjectile()
    {
        Rigidbody2D laser = Instantiate(laserProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation) as Rigidbody2D;

        laser.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileSpeed);

    }
}
