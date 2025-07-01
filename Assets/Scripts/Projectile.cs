using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 250.0f;
    private float timeToLive = 1f;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.AddForce(transform.up * projectileSpeed);
        Destroy(gameObject, timeToLive);
    }

}
