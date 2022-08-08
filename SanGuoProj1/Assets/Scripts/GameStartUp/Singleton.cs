using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static Singleton<T> Instance
    {
        get;
        private set;
    }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Instance Already Exist!");
            Destroy(this);
            return;
        }
        Instance = this;
    }

}