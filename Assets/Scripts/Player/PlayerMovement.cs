using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStatsSO PlayerStats;

    private InputAction m_moveAction;
    private Vector2 m_moveValue;
    private Rigidbody2D m_rb;
    private PlayerVFX m_playerVFX;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_playerVFX = GetComponent<PlayerVFX>();
        m_moveAction = InputSystem.actions.FindAction("Move");
    }

    public void Move()
    {
        m_moveValue = m_moveAction.ReadValue<Vector2>().normalized;

        float thrusterEmmissionRate = Mathf.Abs(m_moveValue.x * 30);
        m_playerVFX.ThrustersEmitFire(thrusterEmmissionRate);

        m_rb.AddForce(m_moveValue * PlayerStats.ThrustForce);

        // This is to stop the player for accelerating if the move button is constantly pressed.
        if (m_rb.linearVelocity.magnitude > PlayerStatsSO.k_MaxSpeed)
        {
            m_rb.linearVelocity = m_rb.linearVelocity.normalized * PlayerStatsSO.k_MaxSpeed;
        }
    }

}