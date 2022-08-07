using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager  m_instance;

    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                Debug.LogError("GameManager is null!");
            }

            return m_instance;
        }
    }

    private void Awake()
    {
        m_instance = this;
    }

    enum GameState
    {
        WAIT,
        START,
        OVER,
    }

    private GameState m_gameState;

    private void Start()
    {
        m_gameState = GameState.WAIT;
    }
}
