using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidBody;
    private bool m_isMoveable = true;
    private bool m_isInputActive = false;
    private float m_horizontalMoveInput;
    private Animator m_animator;

    [SerializeField] private float m_horizontalMoveSpeed = 1.0f;

    private void OnEnable()
    {
        EventManager.StartListening(EventName.CATCH, OnTargetCatch);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.CATCH, OnTargetCatch);
    }

    void OnTargetCatch()
    {
        m_animator.SetTrigger("Catch");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("BouncePad"), LayerMask.NameToLayer("Player"), true);
    }
    
    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        m_horizontalMoveInput = Input.GetAxisRaw("Horizontal");
        if (m_horizontalMoveInput != 0)
        {
            m_rigidBody.MovePosition(m_rigidBody.position + Vector2.right * m_horizontalMoveSpeed * m_horizontalMoveInput * Time.fixedDeltaTime);
        }
        else
        {
            ResetForce();
        }
    }

    void ResetForce()
    {
        m_rigidBody.velocity = Vector2.zero;
        m_rigidBody.angularVelocity = 0.0f;
    }
}