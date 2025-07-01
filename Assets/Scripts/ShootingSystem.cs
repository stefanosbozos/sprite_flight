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
    [SerializeField] private int ammoAmount = 10;
    [SerializeField] private float reloadTime = 0f;
    [SerializeField] private float reloadCooldown = 0.5f;

    // Player Input system
    Camera mainCam;
    Vector3 mousePos;
    InputAction shoot;
    InputAction reload;
    private float rotationOffset = 90.0f;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        shoot = InputSystem.actions.FindAction("Shoot");
        reload = InputSystem.actions.FindAction("Reload");
    }

    void Update()
    {
        Aim();
        Shoot();
        Reload();
    }

    void Aim()
    {
        mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.value);

        Vector3 playerRotation = mousePos - transform.position;

        float rotationZ = Mathf.Atan2(playerRotation.y, playerRotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - rotationOffset);
    }

    void Shoot()
    {
        if (shoot.WasPressedThisFrame() && ammoAmount > 0)
        {
            // Solution at https://discussions.unity.com/t/spawning-a-projectile-in-front-of-a-player-based-on-player-rotation/165403
            Instantiate(laserProjectile, transform.Find("LaserSpawn").position, transform.Find("LaserSpawn").rotation);
            ammoAmount -= 1;
            Debug.Log("Ammo:" + ammoAmount);
        }
        if (ammoAmount == 0)
        {
            //Debug.Log("Out of Ammo - Reload");
        }
    }

    void Reload()
    {
        reloadTime += Time.deltaTime;
        if (reload.WasPerformedThisFrame() && ammoAmount <= 0)
        {
            reloadTime = 0f;
            Debug.Log("RELOADING!");
            if (reloadTime >= reloadCooldown)
            {
                ammoAmount = 10;
                Debug.Log("Ready!");
            }
        }
    }

}
