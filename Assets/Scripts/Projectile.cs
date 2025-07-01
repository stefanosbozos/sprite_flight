using UnityEngine;
using UnityEngine.Animations;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1f;
    Vector3 direction = Vector3.up;
    private float timeToLive = 1.5f;

    void Update()
    {
        transform.Translate(direction * projectileSpeed * Time.deltaTime);  
        //rb.AddForce(transform.up * projectileSpeed);
        Destroy(gameObject, timeToLive);
    }

}
