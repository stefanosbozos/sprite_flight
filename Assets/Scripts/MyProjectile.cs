using UnityEngine;

public class MyProjectile : MonoBehaviour
{
    [SerializeField]
    private float projectileSpeed = 1f;
    [SerializeField]
    Vector3 direction = Vector3.up;
    private float timeToLive = 1.5f;

    void Update()
    {
        transform.Translate(direction * projectileSpeed * Time.deltaTime);  
        Destroy(gameObject, timeToLive);
    }

}
