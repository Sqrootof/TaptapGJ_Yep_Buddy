using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIntance<T> : MonoBehaviour where T : TIntance<T>, new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null){
            instance = (T)this;
        }
        else Destroy(this);
    }
}
