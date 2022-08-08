using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    WAIT,
    START,
    OVER,
}

public class GameManager : Singleton<GameManager>
{
    private GameState m_gameState = GameState.WAIT;

    public GameState CurrentGameState => m_gameState;
    
    public static event Action<GameState> OnGameStateChanged;


    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        GameStartComponent.OnGameStart += StartGame;
    }

    private void OnDisable()
    {
        GameStartComponent.OnGameStart -= StartGame;
    }

    private void UpdateGameState(GameState state)
    {
        if (state == m_gameState)
        {
            return;
        }
        m_gameState = state;
        switch (state)
        {
            case GameState.WAIT:
                break;
            case GameState.START:
                break;
            case GameState.OVER:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        OnGameStateChanged?.Invoke(state);
    }

    private void StartGame()
    {
        UpdateGameState(GameState.START);
    }
}
