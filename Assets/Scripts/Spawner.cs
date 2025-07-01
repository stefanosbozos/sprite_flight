using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    [SerializeField] private float timeBetweenSpawns = 2f;
    private float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenSpawns)
        {
            Instantiate(enemy);
            timer = 0f;
        }   
    }
}
