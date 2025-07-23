using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRuntimeSet", menuName = "Enemy Runtime Set")]
public class EnemyRuntimeSetSO : ScriptableObject
{
    private static List<GameObject> m_enemies = new List<GameObject>();
    public static List<GameObject> Enemies => m_enemies;

    public void Add(GameObject enemyToAdd)
    {
        if (!m_enemies.Contains(enemyToAdd))
        {
            m_enemies.Add(enemyToAdd);
        }
    }

    public void Remove(GameObject enemy)
    {
        if (m_enemies.Contains(enemy))
        {
            m_enemies.Remove(enemy);
        }
    }

    public static int Size()
    {
        return m_enemies.Count;
    }

    public static void KillAll()
    {
        foreach (var enemy in m_enemies)
        {
            Destroy(enemy);
        }
    }
}