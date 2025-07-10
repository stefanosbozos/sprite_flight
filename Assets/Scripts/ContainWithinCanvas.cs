using UnityEngine;

public class ContainWithinCanvas : MonoBehaviour
{
    public RectTransform canvasRect;
    public float min_limit_X, max_limit_X, min_limit_Y, max_limit_Y;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasRect = GetComponent<RectTransform>();
        min_limit_X = canvasRect.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        ClampPositionWithinScreen();
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
