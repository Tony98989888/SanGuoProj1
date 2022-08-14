using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : new()
{
    private static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new T();
            }

            return m_instance;
        }
    }

    protected void OnDestroy()
    {
        m_instance = default;
    }
}