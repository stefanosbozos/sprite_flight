using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : MonoBehaviour
{
    [SerializeField] private uint m_initialPoolSize;
    [SerializeField] private PooledObject m_objectToPool;

    private Stack<PooledObject> stack;

    private void Start()
    {
        SetupPool();
    }

    // Creates the pool (invoike only when the the lag is not noticeable)
    public void SetupPool()
    {
        stack = new Stack<PooledObject>();
        PooledObject instance = null;

        for (int i = 0; i < m_initialPoolSize; i++)
        {
            instance = Instantiate(m_objectToPool);
            instance.Pool = this;
            instance.gameObject.SetActive(false);
            stack.Push(instance);
            Debug.Log("Instantiated Pool");
        }
    }

    public PooledObject GetPooledObject()
    {
        // if the pool is not enough instantiate new objects
        if (stack.Count == 0)
        {
            PooledObject newInstance = Instantiate(m_objectToPool);
            newInstance.Pool = this;
            return newInstance;
        }

        PooledObject nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(PooledObject pooledObject)
    {
        stack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }
}