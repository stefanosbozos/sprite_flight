using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class ContainWithinCanvas : MonoBehaviour
{
    private VirtualMouseInput virtualMouse;

    void Awake()
    {
        virtualMouse = GetComponent<VirtualMouseInput>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
            Solution at:
            https://www.youtube.com/watch?v=j2XyzSAD4VU&t=293s
        */
        Vector2 virtualMousePosition = virtualMouse.virtualMouse.position.value;
        virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0, Screen.width);
        virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0, Screen.height);
        InputState.Change(virtualMouse.virtualMouse.position, virtualMousePosition);
    }
    
}
