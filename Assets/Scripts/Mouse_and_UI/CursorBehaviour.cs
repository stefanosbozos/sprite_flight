using UnityEngine;
using UnityEngine.InputSystem;

public class CursorBehaviour : MonoBehaviour
{
    // Change the mouse pointer look
    public Texture2D cursorTexture;
    public GameObject virtualMouse;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Cursor.visible = false;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        virtualMouse = GameObject.FindGameObjectWithTag("virtual_mouse");
        ShowVirtualMouse();
    }

    void ShowVirtualMouse()
    {
        if (Gamepad.current != null)
        {
            virtualMouse.SetActive(true);
        }
        else
        {
            virtualMouse.SetActive(false);
        }
    }
}
