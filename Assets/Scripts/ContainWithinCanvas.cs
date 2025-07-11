using UnityEngine;
using UnityEngine.InputSystem;

public class ContainWithinCanvas : MonoBehaviour
{
    InputAction virtualMouse;
    public RectTransform canvasRect;
    public float min_limit_X, max_limit_X, min_limit_Y, max_limit_Y;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasRect = GetComponent<RectTransform>();
        virtualMouse = InputSystem.actions.FindAction("aim");
        min_limit_X = canvasRect.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        ClampPositionWithinScreen();

        Vector3 v_mousePos = virtualMouse.ReadValue<Vector2>();
        Vector3 v_mouseScreenPos = Camera.main.ScreenToWorldPoint(new Vector3(v_mousePos.x, v_mousePos.y, Camera.main.nearClipPlane));


    }
    
    void ClampPositionWithinScreen()
    {
        if (transform.position.x >= max_limit_X)
        {
            transform.position = new Vector3(max_limit_X, transform.position.y, transform.position.z);
        }

        if (transform.position.x <= min_limit_X)
        {
            transform.position = new Vector3(min_limit_X, transform.position.y, transform.position.z);
        }

        if (transform.position.y >= max_limit_Y)
        {
            transform.position = new Vector3(transform.position.x, max_limit_Y, transform.position.z);    
        }

        // This needs refactoring
        if (transform.position.y <= min_limit_Y)
        {
            transform.position = new Vector3(transform.position.x, min_limit_Y, transform.position.z);
        }
    }
}
