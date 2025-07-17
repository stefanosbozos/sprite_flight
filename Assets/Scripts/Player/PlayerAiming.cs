using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    private InputAction m_aim;
    private int rotationOffset = 90;

    void Awake()
    {
        m_aim = InputSystem.actions.FindAction("Aim");
    }

    public void Aim()
    {
        Vector3 mouseScreenPos = m_aim.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));

        Vector3 playerDirection = mouseWorldPos - transform.position;
        playerDirection.z = 0; // Ensure that the player does not rotate on the Z-axis

        if (playerDirection.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - rotationOffset);
        }
    }
}