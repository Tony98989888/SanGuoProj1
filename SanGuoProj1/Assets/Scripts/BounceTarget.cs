using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BounceTarget : ICatchable
{
    [SerializeField] private float m_speedFactor;

    private Rigidbody2D m_rigidBody;

    [SerializeField] private LayerMask m_bouncePadLayer;
    [SerializeField] private LayerMask m_catcherLayer;
    [SerializeField] private LayerMask m_grabAreaLayer;

    public bool IsBounced => m_isBounced;

    private Vector2 m_lastFrameVelocity;

    private bool m_isSpawned = false;
    private bool m_isBounced = false;

    //private Vector3 m_targetDir;

    // Start is called before the first frame update
    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("BounceTarget"), LayerMask.NameToLayer("BounceTarget"),
            true);
    }

    // Update is called once per frame
    void Update()
    {
        m_lastFrameVelocity = m_rigidBody.velocity;
        // Debug.DrawLine(transform.position, transform.position + m_targetDir* 50, Color.magenta);
    }

    // private void FixedUpdate()
    // {
    //     if (m_targetDir != null)
    //     {
    //         m_rigidBody.MovePosition(transform.position + m_targetDir.normalized * (GameManager.BounceTargetSpeed * Time.deltaTime));
    //     }
    // }

    private void OnEnable()
    {
        EventManager.StartListening(EventName.LAUNCHER_ANIMATION_FINISHED, OnThrowAnimationFinished);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.LAUNCHER_ANIMATION_FINISHED, OnThrowAnimationFinished);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if ((m_bouncePadLayer & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
        {
            Debug.Log(col.gameObject.name);
            var reflectVec = Vector2.Reflect(m_lastFrameVelocity, col.contacts[0].normal).normalized;
            Debug.DrawRay(col.contacts[0].point, -m_lastFrameVelocity, Color.blue, 2f);
            Debug.DrawRay(col.contacts[0].point, col.contacts[0].normal, Color.yellow, 2f);
            Debug.DrawRay(col.contacts[0].point, reflectVec, Color.red, 2f);
             m_speedFactor += 0.1f;
            m_rigidBody.velocity = Vector2.zero;
            m_rigidBody.velocity = reflectVec.normalized * GameManager.BounceTargetSpeed;
            // m_targetDir = new Vector3(reflectVec.x, reflectVec.y, transform.position.z);
            // Debug.Log(m_targetDir);
            EventManager.TriggerEvent(EventName.ON_BOUNCE_TARGET_BOUNCE, this.gameObject);
            m_isBounced = true;
        }

        // if ((m_catcherLayer & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
        // {
        //     Debug.Log("Hit Player!!");
        //     EventManager.TriggerEvent(EventName.CATCH);
        //     Destroy(gameObject);
        // }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((m_grabAreaLayer & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
        {
            EventManager.TriggerEvent(EventName.ON_GRAB_AREA_ENTER, this.gameObject);
        }
    }

    private void OnThrowAnimationFinished(GameObject param)
    {
        if (m_isSpawned)
        {
            return;
        }

        var launcher = GameObject.FindWithTag("Launcher").transform;
        // transform.position = launcher.transform.position;
        var randomDir = MathHelper.RandomVector2BetweenAngle(130.0f * Mathf.Deg2Rad, 50.0f * Mathf.Deg2Rad);
        //m_targetDir = new Vector3(randomDir.x, -Mathf.Abs(randomDir.y), launcher.position.z);
        m_rigidBody.velocity =
            (new Vector3(randomDir.x, -Mathf.Abs(randomDir.y), launcher.position.z))
            .normalized * GameManager.BounceTargetSpeed;
        // use kinematic
        Debug.DrawRay(launcher.position,
            (new Vector3(randomDir.x, -Mathf.Abs(randomDir.y), launcher.position.z)).normalized,
            Color.green, 2f);
        Debug.DrawRay(launcher.position,
            new Vector3(Mathf.Cos(50.0f * Mathf.Deg2Rad), Mathf.Sin(50.0f * Mathf.Deg2Rad), launcher.position.z),
            Color.yellow,
            2f);
        Debug.DrawRay(launcher.position,
            new Vector3(Mathf.Cos(130.0f * Mathf.Deg2Rad), Mathf.Sin(130.0f * Mathf.Deg2Rad), launcher.position.z),
            Color.yellow, 2f);
        m_isSpawned = true;
    }

    public void OnCatch()
    {
        m_rigidBody.isKinematic = true;
    }
}