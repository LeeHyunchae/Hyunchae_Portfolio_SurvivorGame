using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<Type, ObjectPool<IPoolable>> poolDict = new Dictionary<Type, ObjectPool<IPoolable>>();
    // TODO :: pool Init time?

    ~PoolManager()
    {
        poolDict.Clear();
        poolDict = null;
    }

    public ObjectPool<IPoolable> GetObjectPool<T>() where T : IPoolable, new()
    {
        poolDict.TryGetValue(typeof(T), out var pool);

        if(pool == null)
        {
            pool = CreateObjectPool<T>();
        }

        return pool;
    }

    private ObjectPool<IPoolable> CreateObjectPool<T>() where T : IPoolable, new()
    {
        var pool = new ObjectPool<IPoolable>();
        Type type = typeof(T);

        poolDict.Add(type, pool);

        return pool;
    }

    public void RemoveObjectPool<T>() where T : IPoolable, new()
    {
        Type type = typeof(T);
        if(poolDict.TryGetValue(type, out var pool))
        {
            pool.OnRelease();
            poolDict.Remove(type);
        }
        else
        {
            Debug.Log("Not Found Pool");
        }
    }

    //private DataObjectPool<T> CreatePool<T>() where T : IPoolable, new()
    //{
    //    DataObjectPool<T> newPool = new DataObjectPool<T>(DEFAULT_POOL_SIZE);
    //    poolList.Add(newPool);
    //    return newPool;
    //}

    //public DataObjectPool<T> GetPool<T>() where T : IPoolable, new()
    //{
    //    int listSize = poolList.Capacity;

    //    for(int i = 0; i <listSize; i++)
    //    {
    //        Type t = poolList[i].GetType();
    //        if(typeof(DataObjectPool<T>).Equals(t))
    //        {
    //            Debug.Log("Already Create Pool");
    //            return (DataObjectPool<T>)poolList[i];
    //        }
    //    }

    //    Debug.Log("Create New Pool");
    //    return CreatePool<T>();
    //}
}
