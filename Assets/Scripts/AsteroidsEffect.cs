using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;

/**
    This script tries to make an "Asteroids" effect on the gameobjects that is being applied.
    When the gameobject moves off the screen, it teleports to the opposite side of the same axis.
    Found it here:
    https://code.tutsplus.com/create-an-asteroids-like-screen-wrapping-effect-with-unity--gamedev-15055a
*/

public class AsteroidsEffect : MonoBehaviour
{
    // Here we will store the renderer points of the object
    private Renderer[] renderers;


    private Camera m_MainCamera;
    // Store the World to Viewport Point position of the object
    private Vector3 viewportPosition;

    // These will be used to position the ghost ships to create the
    // screen wrapping effect.
    private Vector3 screenBottomLeft;
    private Vector3 screenTopRight;
    private float screenWidth;
    private float screenHeight;

    // We will store 8 ghost player ships to be positioned around the screen view
    private Transform[] ghosts;

    private bool isWrappingX = false;
    private bool isWrappingY = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        m_MainCamera = Camera.main;

        // Find the coordinates of the top right and the bottom left of the screen
        screenBottomLeft = m_MainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, transform.position.z));
        screenTopRight = m_MainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, transform.position.z));

        // Calculate the width and height of the screen
        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.y - screenBottomLeft.y;

        ghosts = new Transform[8];
    }

    // Update is called once per frame
    void Update()
    {
        ScreenWrapping();
        CreateGhostShips();
        PositionGhostShips();
    }

    // This is to check whether the object is off the screen
    // In the below we check if the renderers of the object are visible
    bool CheckRenderers()
    {
        foreach (Renderer renderer in renderers)
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }

        return false;
    }

    void ScreenWrapping()
    {

        if (CheckRenderers())
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingX && isWrappingY)
        {
            return;
        }

        viewportPosition = m_MainCamera.WorldToViewportPoint(transform.position).normalized;
        Vector3 newPosition = transform.position;

        if (!isWrappingX && (viewportPosition.x >= 1 || viewportPosition.x <= 0))
        {
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }

        if (!isWrappingY && (viewportPosition.y >= 1 || viewportPosition.y <= 0))
        {
            newPosition.y = -newPosition.y;
            isWrappingY = true;
        }

        transform.position = newPosition;

    }

    void CreateGhostShips()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i] = Instantiate(transform, Vector3.zero, Quaternion.identity);
            DestroyImmediate(ghosts[i].GetComponent<Transform>());
        }
    }

    void PositionGhostShips()
    {
        // All the ghosts will be relative to this object's position
        Vector3 ghostPosition = transform.position;

        // Right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[0].position = ghostPosition;

        // Bottom Right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[1].position = ghostPosition;

        // Bottom
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[2].position = ghostPosition;

        //Bottom Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[3].position = ghostPosition;

        //Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[4].position = ghostPosition;

        // Top Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[5].position = ghostPosition;

        // Top
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[6].position = ghostPosition;

        // Top Right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[7].position = ghostPosition;

        // Give the ghosts the same rotation as this object
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].rotation = transform.rotation;
        }


    }
    
}
