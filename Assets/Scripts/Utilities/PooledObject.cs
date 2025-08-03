using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private ObjectPool m_pool;
    public ObjectPool Pool { get => m_pool; set => m_pool = value; }

    public void Release()
    {
        m_pool.ReturnToPool(this);
    }
}