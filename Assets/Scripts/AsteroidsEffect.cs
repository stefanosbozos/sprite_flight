using Mono.Cecil.Cil;
using UnityEngine;

/**
    This script tries to make an "Asteroids" effect on the gameobjects that is being applied.
    When the gameobject moves off the screen, it teleports to the opposite side of the same axis.
    Found it here:
    https://code.tutsplus.com/create-an-asteroids-like-screen-wrapping-effect-with-unity--gamedev-15055a
*/

public class AsteroidsEffect : MonoBehaviour
{
    private Renderer[] renderers;

    private Camera m_MainCamera;
    private Vector3 viewportPosition;

    private bool isWrappingX = false;
    private bool isWrappingY = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        ScreenWrapping();
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

        m_MainCamera = Camera.main;
        viewportPosition = m_MainCamera.WorldToViewportPoint(transform.position).normalized;
        Debug.Log(viewportPosition);
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
}
