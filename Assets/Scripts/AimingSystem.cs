using UnityEngine;
using UnityEngine.InputSystem;

public class AimingSystem : MonoBehaviour
{

    InputAction aim;
    public Transform player;
    public Transform aimingSights;
    public float maxOffsetDistance = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        aim = InputSystem.actions.FindAction("aim");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseScreenPos = aim.ReadValue<Vector2>();
        Debug.Log(mouseScreenPos);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));

        Vector3 direction = mouseWorldPos - player.position;
        direction = Vector3.ClampMagnitude(direction, maxOffsetDistance);

        Vector3 clampedMousePos = player.position + direction;
        aimingSights.position = clampedMousePos;
        Debug.DrawLine(player.position, clampedMousePos, Color.green);

    }

}
