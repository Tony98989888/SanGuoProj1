using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidBody;
    private bool m_isMoveable = false;
    private bool m_isInputActive = false;
    private float m_horizontalMoveInput;
    private Animator m_animator;
    
    public enum PlayerState
    {
        WAIT,
        MOVE,
    }

    private PlayerState m_playerState;
    
    [SerializeField] private float m_horizontalMoveSpeed = 1.0f;

    private void OnEnable()
    {
        EventManager.StartListening(EventName.CATCH, OnTargetCatch);
        GameStartComponent.OnGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.CATCH, OnTargetCatch);
        GameStartComponent.OnGameStart -= OnGameStart;
    }

    void OnTargetCatch(object param)
    {
        m_animator.SetTrigger("Catch");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_playerState = PlayerState.WAIT;
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("BouncePad"), LayerMask.NameToLayer("Player"), true);
    }

    private void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        if (m_playerState == PlayerState.WAIT)
        {
            return;
        }
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

    void OnGameStart()
    {
        m_playerState = PlayerState.MOVE;
    }
}