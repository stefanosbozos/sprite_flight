using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveSO", menuName = "Enemy Wave")]

public class EnemyWaveSO : ScriptableObject
{
    public List<GameObject> EnemyType;

    // How many enemies the wave will have in total
    public int EnemiesPerWave;
    // How many enemies will be spawned on screen (spawn in batches)
    public int EnemiesOnScreen;
    public float EnemySpawningInterval;
    public static float m_TimeBetweenEachWave = 3f;
}