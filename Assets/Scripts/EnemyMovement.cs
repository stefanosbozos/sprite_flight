using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    
    private Rigidbody2D rb;

    [SerializeField] private GameObject projectile;
    [SerializeField] private float timeBetweenShots = 1f;
    private float deltaDistance;
    private float timer = 0f;

    [SerializeField] private float movement_speed = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        deltaDistance = Mathf.FloorToInt(Random.Range(5f, 20f));
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        Shoot();
    }

    void FollowPlayer()
    {
        // Go to the players position (Lurker's behaviour)
        Vector3 distanceFromThePlayer = new Vector3(transform.position.x, transform.position.y - deltaDistance, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position - distanceFromThePlayer, movement_speed * Time.deltaTime);

        // Change the rotation accoriding to the player's rotation to always face the player
        Vector3 enemyRotation = player.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(enemyRotation.y, enemyRotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, rotationZ + 90.0f), 3f * Time.deltaTime);
    }

    void Shoot()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenShots)
        {
            GameObject spawnedBullet = Instantiate(projectile, transform.Find("ProjectileSpawn").position, transform.Find("ProjectileSpawn").rotation);
            timer = 0;
            Destroy(spawnedBullet, 3f);
        }
    }
}
