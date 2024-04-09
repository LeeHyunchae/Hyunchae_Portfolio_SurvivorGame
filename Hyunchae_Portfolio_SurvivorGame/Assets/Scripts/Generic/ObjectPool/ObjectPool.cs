using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ObjectPool<T> where T : IPoolable
{
    private Queue<T> objectQueue = null;

    private GameObject originPrefab_GameObj = null;

    private bool isInitilaze = false;

    private int increaseSize;

    public ObjectPool()
    {
        objectQueue = new Queue<T>();
        objectQueue.Clear();
    }

    public void Init(string _prefabPath, int _increaseSize = 4)
    {
        var originPrefab = Resources.Load(_prefabPath);

        originPrefab_GameObj = originPrefab as GameObject;
        increaseSize = _increaseSize;

        IncreasePool();

        isInitilaze = true;
    }

    ~ObjectPool()
    {
        OnRelease();
    }
    public bool IsInit => isInitilaze;

    public void EnqueueObject(T _obj)
    {
        _obj.OnEnqueue();
        objectQueue.Enqueue(_obj);
    }

    public T GetObject()
    {
        T obj;

        if (objectQueue.TryDequeue(out obj))
        {
            return obj;
        }
        else
        {
            IncreasePool();
            return objectQueue.Dequeue();
        }
    }

    private void IncreasePool()
    {
        for(int i = 0; i<increaseSize;i++)
        {
            GameObject obj = GameObject.Instantiate(originPrefab_GameObj);
            T componenet = obj.GetComponent<T>();

            EnqueueObject(componenet);
        }
    }


    public void OnRelease()
    {
        objectQueue.Clear();
        objectQueue = null;
    }
}
