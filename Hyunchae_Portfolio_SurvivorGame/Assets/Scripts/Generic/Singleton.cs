using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class, new()
{
    private static T instance = null;
    public static T getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
                (instance as Singleton<T>).Initialize();
            }

            return instance;
        }
    }

    public virtual bool Initialize()
    {
        return true;
    }
}
