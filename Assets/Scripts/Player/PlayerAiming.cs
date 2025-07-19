using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;

    private Rigidbody2D m_rb;
    private InputAction m_aim;
    private Vector3 aimValue;
    private int rotationOffset = 90;

    void Awake()
    {
        m_aim = InputSystem.actions.FindAction("Aim");
        m_rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Aim();
    }

    private void Aim()
    {
        aimValue = m_aim.ReadValue<Vector2>().normalized;

        if (aimValue.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(aimValue.y, aimValue.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - rotationOffset);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * m_playerStats.RotationSpeed);
        }
    }
}