using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAI", menuName = "Enemy AI")]
public class EnemyAISO : ScriptableObject
{
    float distanceFromPlayerLimit;
    float distanceFromThePlayer;
    float minDistanceFromPlayer = 5.0f;
    float maxDistanceFromPlayer = 15.0f;

    public void GenerateDistanceFromPlayer()
    {
        distanceFromPlayerLimit = Mathf.FloorToInt(Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer));
    }

    public float DistanceFromPlayer => distanceFromThePlayer;
    public float DistanceFromPlayerLimit => distanceFromPlayerLimit;

    public void SetDistanceFromPlayer(float distance)
    {
        distanceFromThePlayer = distance;
    }

}