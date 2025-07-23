using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRuntimeSet", menuName = "Enemy Runtime Set")]
public class EnemyRuntimeSetSO : ScriptableObject
{
    private List<GameObject> m_enemies = new List<GameObject>();
    public List<GameObject> Enemies => m_enemies;

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

    public int Size()
    {
        return m_enemies.Count;
    }

    public void KillAll()
    {
        foreach (var enemy in m_enemies)
        {
            Destroy(enemy);
        }
    }
}