using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public enum GameDevice
    {
        KeyboardMouse,
        Gamepad,
    }

    private GameDevice activeGameDevice;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        InputSystem.onActionChange += InputSystem_OnActionChange;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InputSystem_OnActionChange(object arg1, InputActionChange inputActionChange)
    {
        if (inputActionChange == InputActionChange.ActionPerformed && arg1 is InputAction)
        {
            InputAction inputAction = arg1 as InputAction;
            if (inputAction.activeControl.device.displayName == "VirtualMouse")
            {
                //Ingnore virtual mouse
                return;
            }
            if (inputAction.activeControl.device is Gamepad)
            {
                if (activeGameDevice != GameDevice.Gamepad)
                {
                    ChangeActiveGameDevice(GameDevice.Gamepad);
                }
                else
                {
                    if (activeGameDevice != GameDevice.KeyboardMouse)
                    {
                        ChangeActiveGameDevice(GameDevice.KeyboardMouse);
                    }
                }
            }

        }
    }

    private void ChangeActiveGameDevice(GameDevice activeGameDevice)
    {
        this.activeGameDevice = activeGameDevice;
        Debug.Log("New active game device: " + activeGameDevice);

        Cursor.visible = activeGameDevice == GameDevice.KeyboardMouse;

    }
}
