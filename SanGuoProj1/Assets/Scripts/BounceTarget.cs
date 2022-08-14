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

    private Vector2 m_lastFrameVelocity;

    private bool m_isSpawned = false;

    // Start is called before the first frame update
    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("BounceTarget"), LayerMask.NameToLayer("BounceTarget"), true);
    }

    // Update is called once per frame
    void Update()
    {
        m_lastFrameVelocity = m_rigidBody.velocity;
    }

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
        Debug.Log(col.gameObject.name);
        if ((m_bouncePadLayer & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
        {
            var reflectVec = Vector2.Reflect(m_lastFrameVelocity, col.contacts[0].normal);
            Debug.DrawRay(col.contacts[0].point, -m_lastFrameVelocity, Color.blue, 2f);
            Debug.DrawRay(col.contacts[0].point, col.contacts[0].normal, Color.yellow, 2f);
            Debug.DrawRay(col.contacts[0].point, reflectVec, Color.red, 2f);
            m_speedFactor += 0.1f;
            m_rigidBody.velocity = reflectVec.normalized * m_speedFactor;
            EventManager.TriggerEvent(EventName.ON_BOUNCE_TARGET_BOUNCE, this.gameObject);
        }

        // if ((m_catcherLayer & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
        // {
        //     Debug.Log("Hit Player!!");
        //     EventManager.TriggerEvent(EventName.CATCH);
        //     Destroy(gameObject);
        // }
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
        m_rigidBody.velocity =
            (new Vector3(randomDir.x, -Mathf.Abs(randomDir.y), launcher.position.z))
            .normalized * m_speedFactor;
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