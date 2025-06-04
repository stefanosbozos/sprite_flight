using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Get a reference to the Rigidbody component
    Rigidbody2D rb;

    [Header("Meteor Size Limits")]
    // The size limits of the obstacle
    [SerializeField] float minSize = 0.5f;
    [SerializeField] float maxSize = 3.0f;

    [Header("Meteor Speed Limits")]
    // The speed of the obstacle
    [SerializeField] float minSpeed = 50f;
    [SerializeField] float maxSpeed = 150f;

    [Header("Meteor Spin Speed Limit")]
    // The spin applied to the obstacle
    [SerializeField] float maxSpinSpeed = 10f;

    [Header("Impact Effects")]
    [SerializeField] private GameObject impactExplosion;

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the point of impact on the object
        Vector2 contactPoint = collision.GetContact(0).point;

        // Instantiate the particle in the world
        GameObject bounceEffect = Instantiate(impactExplosion, contactPoint, Quaternion.identity);

        // Destroy after 1 second
        Destroy(bounceEffect, 1f);
    }

}
