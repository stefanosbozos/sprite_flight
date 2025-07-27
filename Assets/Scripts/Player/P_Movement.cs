using UnityEngine;
using UnityEngine.InputSystem;

public class P_Movement : Player
{
    [SerializeField] protected float m_thrustForce;
    private InputAction m_moveAction;
    private Vector2 m_moveValue;


    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_vfx = GetComponent<VisualEffects>();
        m_moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Move();
        MaintainLinearVelocity();
    }

    private void Move()
    {
        m_moveValue = m_moveAction.ReadValue<Vector2>().normalized;

        m_vfx.ThrustersEmitFire(m_rigidBody.linearVelocity.sqrMagnitude);

        m_rigidBody.AddForce(m_moveValue * m_thrustForce);
    }

}