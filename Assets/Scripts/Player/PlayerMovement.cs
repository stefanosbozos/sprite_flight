using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;
    private InputAction m_moveAction;
    private Vector2 m_moveValue;
    private Rigidbody2D m_rb;
    private VisualEffects m_playerVFX;

    private const float k_MaxSpeed = 20;


    void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_playerVFX = GetComponent<VisualEffects>();
        m_moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        m_moveValue = m_moveAction.ReadValue<Vector2>().normalized;

        m_playerVFX.ThrustersEmitFire(m_rb.linearVelocity.sqrMagnitude);

        m_rb.AddForce(m_moveValue * m_playerStats.ThrustForce);

        // This is to stop the player for accelerating if the move button is constantly pressed.
        if (m_rb.linearVelocity.magnitude > k_MaxSpeed)
        {
            m_rb.linearVelocity = m_rb.linearVelocity.normalized * k_MaxSpeed;
        }
    }

}