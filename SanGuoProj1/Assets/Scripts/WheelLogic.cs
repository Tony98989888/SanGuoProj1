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

    [SerializeField] private Collider2D m_collider;

    private Rigidbody2D m_rigidbody;

    private bool m_isLeft = false;

    private Vector3 m_targetPos;
    private Vector3 m_startPos;
    private bool m_ready = false;

    [SerializeField] private float m_speed = 1.0f;

    [SerializeField] private float m_minDuration = 2.0f;
    [SerializeField] private float m_maxDuration = 5.0f;

    private Vector3 m_moveDir;

    [SerializeField] private float m_wheelMoveSpeed;

    private void GenerateWayPoints()
    {
        m_collider.enabled = true;
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

        m_moveDir = (m_targetPos - m_startPos).normalized;
        m_ready = true;
    }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!m_ready)
        {
            return;
        }

        Debug.DrawLine(m_targetPos, m_startPos);
        if (Vector3.Distance(transform.position, m_targetPos) < 2f)
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

    // private void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (col.gameObject.CompareTag("Catchable"))
    //     {
    //         this.m_collider.enabled = false;
    //     }
    // }
    //
    // private void OnTriggerExit2D(Collider2D col)
    // {
    //     if (col.gameObject.CompareTag("Catchable"))
    //     {
    //         this.m_collider.enabled = true;
    //     }
    // }

    private void FixedUpdate()
    {
        if (m_ready && m_targetPos != null)
        {
            m_rigidbody.MovePosition(transform.position + m_moveDir * Time.deltaTime * m_wheelMoveSpeed);
        }
    }
}