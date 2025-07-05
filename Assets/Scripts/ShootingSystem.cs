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
    [SerializeField] private GameObject gunMuzzle;
    [SerializeField] private GameObject laserProjectile;
    // The amount of heat that the laser can withstand before shutting down to cool off
    [SerializeField] private float cooldownTime = 3f;
    // The step that the laser coolsdown while not overheated and not fired
    [SerializeField] private float coolingOffStep = 3f;
    [SerializeField] private float increasingTemperatureStep = 3f;

    [Header("Aiming Sensitivity")]
    // How quick the player rotates - This will be controlled also in the options menu
    [SerializeField] private float aimSensitivity = 200f;

    // Firing timing
    // The time in seconds between the shots
    [SerializeField] private float timeBetweenShots = 0.05f;
    // Store the time that passed in second since the last shot.
    private float timeSinceLastShot = 0f;
    private bool readyToShoot = true;



    // Player Input system
    Camera mainCam;
    Vector3 mousePos;
    InputAction shoot;
    // Aiming system for gamepad only
    InputAction aim_gp;

    // This is used to make sure that the mouse is aiming from the Y axis of the player.
    private float rotationOffset = 90.0f;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        shoot = InputSystem.actions.FindAction("Shoot");
        aim_gp = InputSystem.actions.FindAction("Gamepad_Aim");
        gunMuzzle.SetActive(false);
        // Debug.Log(InputSystem.GetDevice<Gamepad>());
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
        if (InputSystem.GetDevice<Gamepad>() == null)
        {
            // Mouse input only
            mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.value);
            Vector3 playerRotation = mousePos - transform.position;
            float rotationZ = Mathf.Atan2(playerRotation.y, playerRotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, 0f, rotationZ - rotationOffset), Time.deltaTime * aimSensitivity);
        }
        else
        {
            // Gamepad and Mobile input
            Vector2 playerRotation_gp = aim_gp.ReadValue<Vector2>().normalized;
            if (aim_gp.inProgress)
            {
                float rotationZ_gp = Mathf.Atan2(playerRotation_gp.y, playerRotation_gp.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ_gp - rotationOffset), Time.deltaTime * aimSensitivity);
            }
        }
    }

    // Instantiate a laser PreFab on the screen
    void Shoot()
    {
        timeSinceLastShot += Time.deltaTime;
        if (shoot.inProgress && readyToShoot)
        {
            if (timeSinceLastShot >= timeBetweenShots)
            {
                if (!laserOverheated)
                {
                    // Solution at https://discussions.unity.com/t/spawning-a-projectile-in-front-of-a-player-based-on-player-rotation/165403
                    Instantiate(laserProjectile, transform.Find("LaserSpawn").position, transform.Find("LaserSpawn").rotation);
                    gunMuzzle.SetActive(true);
                    laserTemp += increasingTemperatureStep;
                    //Debug.Log("Laser temp: " + laserTemp);
                }
                else
                {
                    Debug.Log("Laser is overheated!!!");
                }
                readyToShoot = !readyToShoot;
                timeSinceLastShot = 0f;
            }
        }

        if (shoot.WasCompletedThisFrame())
        {
            gunMuzzle.SetActive(false);
        }

        // if 0.5s have elapsed since the last shot, the player is ready to shoot.
        if (timeSinceLastShot >= timeBetweenShots)
        {
            readyToShoot = !readyToShoot;
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

    public float GetLaserTemp()
    {
        return laserTemp;
    }

    public bool IsOverheated()
    {
        return laserOverheated;
    }

}
