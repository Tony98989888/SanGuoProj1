using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private int m_bounceTargetBounceTime = 0;

    private int m_score = 0;

    [SerializeField]
    private static float m_bounceTargetSpeed = 12.0f;

    public static float BounceTargetSpeed
    {
        get { return m_bounceTargetSpeed;}
        set { m_bounceTargetSpeed = value; }
    }

    [SerializeField]
    private TMP_Text m_scoreText;
    
    public static event Action<GameState> OnGameStateChanged;

    private void OnEnable()
    {
        GameStartComponent.OnGameStart += StartGame;
        EventManager.StartListening(EventName.ON_BOUNCE_TARGET_BOUNCE, OnBounceTargetBounce);
        EventManager.StartListening(EventName.ON_GRAB_AREA_ENTER, OnGrabAreaEnter);
    }

    private void OnDisable()
    {
        GameStartComponent.OnGameStart -= StartGame;
        EventManager.StopListening(EventName.ON_BOUNCE_TARGET_BOUNCE, OnBounceTargetBounce);
        EventManager.StopListening(EventName.ON_GRAB_AREA_ENTER, OnGrabAreaEnter);
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
        m_scoreText.text = m_bounceTargetBounceTime.ToString();
    }

    private void OnBounceTargetBouce()
    {
        m_score -= 5;
        m_scoreText.text = m_score.ToString();
    }

    private void OnBounceTargetCatched()
    {
        m_score += 20;
        m_scoreText.text = m_score.ToString();
    }

    private void OnBounceTargetBounce(GameObject obj)
    {
        m_bounceTargetBounceTime += 1;
        m_scoreText.text = m_bounceTargetBounceTime.ToString();
        //m_bounceTargetSpeed += 2.0f;
    }

    private void OnGrabAreaEnter(GameObject obj)
    {
        m_bounceTargetSpeed += 0.5f;
        if (m_bounceTargetSpeed > 20.0f)
        {
            m_bounceTargetSpeed = 20.0f;
        }
    }
}
