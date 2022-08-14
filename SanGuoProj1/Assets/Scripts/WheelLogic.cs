using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WheelLogic : MonoBehaviour
{
    [SerializeField] private List<Transform> m_wayPointsLeft;
    [SerializeField] private List<Transform> m_wayPointsRight;

    private bool m_isLeft = false;

    private Vector3 m_targetPos;
    private Vector3 m_startPos;
    private bool m_ready = false;

    [SerializeField] private float m_speed = 1.0f;

    [SerializeField] private float m_minDuration = 2.0f;
    [SerializeField] private float m_maxDuration = 5.0f;

    private void GenerateWayPoints()
    {
        bool isStartLeft = Random.Range(0.0f, 1.0f) >= 0.5f;
        if (isStartLeft)
        {
            // Vector3 startPos = m_wayPointsLeft[Random.Range(0, m_wayPointsLeft.Count)].position;
            // transform.position = new Vector2(startPos.x, startPos.y);
            m_startPos = transform.position = m_wayPointsLeft[Random.Range(0, m_wayPointsLeft.Count)].position;
            m_targetPos = m_wayPointsRight[Random.Range(0, m_wayPointsRight.Count)].position;
        }
        else
        {
            // Vector3 startPos = m_wayPointsRight[Random.Range(0, m_wayPointsRight.Count)].position;
            // transform.position = new Vector2(startPos.x, startPos.y);
            m_startPos = transform.position = m_wayPointsRight[Random.Range(0, m_wayPointsRight.Count)].position;
            m_targetPos = m_wayPointsLeft[Random.Range(0, m_wayPointsLeft.Count)].position;
        }

        m_ready = true;
    }

    private void Update()
    {
        if (!m_ready)
        {
            return;
        }

        Debug.DrawLine(m_targetPos, m_startPos);
        transform.position = Vector3.MoveTowards(transform.position, m_targetPos, m_speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, m_targetPos) < 0.1f)
        {
            m_ready = false;
            StartCoroutine(MoveWheel());
        }
    }

    IEnumerator MoveWheel()
    {
        yield return new WaitForSeconds(Random.Range(m_minDuration, m_maxDuration));
        GenerateWayPoints();
    }

    private void OnEnable()
    {
        GameStartComponent.OnGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        GameStartComponent.OnGameStart -= OnGameStart;
    }

    void OnGameStart()
    {
        StartCoroutine(MoveWheel());
    }
}