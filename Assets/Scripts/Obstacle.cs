using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Get a reference to the Rigidbody component
    Rigidbody2D rb;
    
    // The size limits of the obstacle
    float minSize = 0.5f;
    float maxSize = 3.0f;

    // The speed of the obstacle
    float minSpeed = 50f;
    float maxSpeed = 150f;

    // The spin applied to the obstacle
    float maxSpinSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody2D>();

        //Randomize the size of the obstacle on Start
        float randomSize = Random.Range(minSize, maxSize);

        // Randomize the speed of the added force on the obstacles. We divide with the randomSize to apply different speeds for different size objects
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;

        // Add a randomized spin on the obstacles - (.AddTorque - https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Rigidbody.AddTorque.html)
        float randomSpin = Random.Range(-maxSpinSpeed, +maxSpinSpeed);
        rb.AddTorque(randomSpin);

        // Randomize the force direction (.insideUnitCircle - https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Random-insideUnitSphere.html )
        Vector2 randomForceDirection = Random.insideUnitCircle;

        // Change the size of the obstacle on Start
        transform.localScale = new Vector3(randomSize, randomSize, 1);
        
        // Vector2.right = move right along the x-axis
        rb.AddForce(randomForceDirection * randomSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
