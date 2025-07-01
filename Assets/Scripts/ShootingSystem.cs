using UnityEngine;
using UnityEngine.InputSystem;

/*
    This script handles the projectile shooting system of the player.
    It has a cooldown so the shots can be fired every .5f seconds.
*/

public class ShootingSystem : MonoBehaviour
{

    // Laser System Variables
    const float HEAT_LIMIT = 100.0f;
    private bool laserOverheated = false;
    // The temperature of the laser
    private float laserTemp = 0;

    [Header("Laser System")]
    [SerializeField] private Rigidbody2D laserProjectile;
    // The amount of heat that the laser can withstand before shutting down to cool off
    [SerializeField] private float cooldownTime = 3f;
    // The step that the laser coolsdown while not overheated and not fired
    [SerializeField] private float coolingOffStep = 3f;



    // Player Input system
    Camera mainCam;
    Vector3 mousePos;
    InputAction shoot;
    // This is used to make sure that the mouse is aiming from the Y axis of the player.
    private float rotationOffset = 90.0f;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        shoot = InputSystem.actions.FindAction("Shoot");
    }

    void Update()
    {
        Aim();
        Shoot();
        LaserHeatingCheck();
    }


    // The aiming system to point the Y axis of the player where the mouse points to.
    void Aim()
    {        
        mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.value);
        Vector3 playerRotation = mousePos - transform.position;

        float rotationZ = Mathf.Atan2(playerRotation.y, playerRotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - rotationOffset);
    }

    // Instantiate a laser PreFab on the screen
    void Shoot()
    {
        if (shoot.WasPressedThisFrame())
        {
            if (!laserOverheated)
            {
                // Solution at https://discussions.unity.com/t/spawning-a-projectile-in-front-of-a-player-based-on-player-rotation/165403
                Instantiate(laserProjectile, transform.Find("LaserSpawn").position, transform.Find("LaserSpawn").rotation);
                laserTemp += 10;
            }
            else
            {
                Debug.Log("Laser is overheated!!!");
            }
        }

    }
    
    // Checks the heat levels of the laser.
    void LaserHeatingCheck()
    {

        if (laserTemp >= 0 && !laserOverheated)
        {
            laserTemp -= Time.deltaTime * coolingOffStep;
        }

        if (laserTemp >= HEAT_LIMIT)
        {
            laserOverheated = true;
            WaitToCooldown();
        }
    }

    // The cooling down of the laser if it is overheated.
    void WaitToCooldown()
    {
        cooldownTime -= Time.deltaTime;
        if (laserOverheated && cooldownTime <= 0.0f)
        {
            laserTemp = 0f;
            laserOverheated = false;
            cooldownTime = 3f;
        }
    }

}
