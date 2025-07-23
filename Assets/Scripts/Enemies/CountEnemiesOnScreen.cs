using UnityEngine;

public class CountEnemiesOnScreen : MonoBehaviour
{
    public EnemyRuntimeSetSO EnemyRuntimeSet;

    void OnEnable()
    {
        EnemyRuntimeSet.Add(gameObject);
    }

    void OnDestroy()
    {
        EnemyRuntimeSet.Remove(gameObject);
    }
}