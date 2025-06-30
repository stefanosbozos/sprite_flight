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

    // Player Input system
    Camera mainCam;
    Vector3 mousePos;
    InputAction shoot;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        shoot = InputSystem.actions.FindAction("Shoot");
    }

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
            Instantiate(laserProjectile, transform.position, transform.rotation);
        }
    }

}
