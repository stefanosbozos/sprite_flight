using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int playerScore;
    private int highScore;

    private float scoreMultiplier = 2.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addScore(int value)
    {
        playerScore += Mathf.FloorToInt(value * scoreMultiplier);
    }
}
