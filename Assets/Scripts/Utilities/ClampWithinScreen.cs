using UnityEngine;

public class ClampWithinScreen : MonoBehaviour
{
    private Camera m_Camera;
    private float cameraSizeX;
    private float cameraSizeY;
    private float limit_X;
    private float limit_Y;

    [Header("Player Screen Limit")]
    [SerializeField] private float X_offset = 1f;
    [SerializeField] private float Y_offset = 3f;
    

    void Awake()
    {
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraSizeX = m_Camera.orthographicSize * 16 / 9;
        cameraSizeY = m_Camera.orthographicSize;
        limit_X = cameraSizeX - X_offset;
        limit_Y = cameraSizeY - Y_offset;
    }

    // Update is called once per frame
    void Update()
    {
        ClampPositionWithinScreen();
    }
    
    void ClampPositionWithinScreen()
    {
        if (transform.position.x >= limit_X)
        {
            transform.position = new Vector3(limit_X, transform.position.y, transform.position.z);
        }

        if (transform.position.x <= -limit_X)
        {
            transform.position = new Vector3(-limit_X, transform.position.y, transform.position.z);
        }

        if (transform.position.y >= limit_Y)
        {
            transform.position = new Vector3(transform.position.x, limit_Y, transform.position.z);    
        }

        // This needs refactoring
        if (transform.position.y <= -limit_Y - 1.5f)
        {
            transform.position = new Vector3(transform.position.x, -limit_Y - 1.5f, transform.position.z);
        }
    }
}
