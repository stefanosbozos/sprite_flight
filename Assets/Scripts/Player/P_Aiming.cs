using UnityEngine;
using UnityEngine.InputSystem;

public class P_Aiming : Player
{
    [SerializeField] protected float m_aimingSpeed;

    private InputAction m_gamepadAim;
    private InputAction m_mouseAim;
    private Vector2 aimValue;
    private int rotationOffset = 90;

    void Awake()
    {
        m_gamepadAim = InputSystem.actions.FindAction("GamepadAim");
        m_mouseAim = InputSystem.actions.FindAction("MouseAim");
    }

    void Update()
    {
        Aim();
    }

    private void Aim()
    {
        if (Gamepad.current != null)
        {
            GamepadAim();
        }
        else
        {
            MouseAim();
        }
    }

    private void GamepadAim()
    {
        aimValue = m_gamepadAim.ReadValue<Vector2>().normalized;

        if (aimValue.sqrMagnitude > 0.1f)
        {
            RotatePlayer(aimValue.y, aimValue.x);
        }
    }

    private void MouseAim()
    {
        aimValue = m_mouseAim.ReadValue<Vector2>();

        // Use player's z-depth for accurate projection
        Vector3 screenPosition = new Vector3(aimValue.x, aimValue.y, Mathf.Abs(Camera.main.transform.position.z));

        Vector3 mouseToWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        mouseToWorldPosition.z = 0f;

        Vector3 mousePointDirection = (mouseToWorldPosition - transform.position).normalized;

        RotatePlayer(mousePointDirection.y, mousePointDirection.x);
        
    }
    private void RotatePlayer(float rotationY, float rotationX)
    {
        float angle = Mathf.Atan2(rotationY, rotationX) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - rotationOffset);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * m_aimingSpeed);
    }

}