using UnityEngine;

/*
    This script implements a screen-wrapping effect like in the game "Asteroids"
    It calculates the screen height and width constraints an it changes the position
    of the object that is applied to to the opposite of the screen.
    
    This implementation was found on this thread:
    https://discussions.unity.com/t/fall-off-left-side-of-screen-and-spawn-on-right/45574/3
*/

public class ScreenWrapping : MonoBehaviour
{
    // X axis constraints
    private float leftConstraint = 0f;
    private float rightConstraint = 0f;

    // Y axis constraints
    private float topConstraint = 0f;
    private float bottomConstraint = 0f;

    // The offset allowed before the object is positioned to the opposite of the screen.
    [SerializeField] private float buffer = 1f;

    Camera m_MainCamera;

    void Awake()
    {
        // Assign the main camera to the camera variable
        m_MainCamera = Camera.main;

        leftConstraint = m_MainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 0 - m_MainCamera.transform.position.z)).x;
        rightConstraint = m_MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0 - m_MainCamera.transform.position.z)).x;

        topConstraint = m_MainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0 - m_MainCamera.transform.position.z)).y;
        bottomConstraint = m_MainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 0 - m_MainCamera.transform.position.z)).y;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ScreenWrap();
    }

    void ScreenWrap()
    {
        // Left side of the screen
        if (transform.position.x < leftConstraint - buffer)
        {
            transform.position = new Vector3(rightConstraint + buffer, transform.position.y, transform.position.z);
        }
        // Right side of the screen
        if (transform.position.x > rightConstraint + buffer)
        {
            transform.position = new Vector3(leftConstraint - buffer, transform.position.y, transform.position.z);
        }
        // Top side of the screen
        if (transform.position.y > topConstraint + buffer)
        {
            transform.position = new Vector3(transform.position.x, bottomConstraint - buffer, transform.position.z);
        }
        // Bottom side of the screen
        if (transform.position.y < bottomConstraint - buffer)
        {
            transform.position = new Vector3(transform.position.x, topConstraint + buffer, transform.position.z);
        }
    }
}
