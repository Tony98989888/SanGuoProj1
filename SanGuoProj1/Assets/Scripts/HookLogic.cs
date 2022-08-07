using UnityEngine;

public class HookLogic : MonoBehaviour
{
    [SerializeField] private bool m_canRotate;

    [SerializeField] private float m_rotateSpeed;

    private float m_rotateAngle = 0.0f;

    private bool m_isRotateRight = true;

    [SerializeField] private float m_maxSideRotateAngle = 90.0f;

    private Vector3 m_startPos;

    [SerializeField] private float m_hookMoveSpeed;

    [SerializeField] private float m_maxHookFlyRange;

    private bool m_canCatch = true;

    private GameObject m_currentCatchObject;

    enum HookState
    {
        Rotate,
        Launch,
        Retract,
    }

    private HookState m_hookState;

    private void OnEnable()
    {
        EventManager.StartListening(EventName.TARGET_CATCHED, OnTargetCatched);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.TARGET_CATCHED, OnTargetCatched);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_startPos = transform.position;
        m_hookState = HookState.Rotate;
    }

    // Update is called once per frame
    void Update()
    {
        DetectInput();
        Rotate();
        MoveHook();
        RetractHook();
    }

    void DetectInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_hookState == HookState.Rotate)
        {
            m_hookState = HookState.Launch;
        }
    }

    void Rotate()
    {
        if (m_hookState != HookState.Rotate) return;
        if (!m_canRotate) return;

        if (m_isRotateRight)
        {
            m_rotateAngle += m_rotateSpeed * Time.deltaTime;
            if (m_rotateAngle >= m_maxSideRotateAngle)
            {
                m_rotateAngle = m_maxSideRotateAngle;
                m_isRotateRight = false;
            }
        }
        else
        {
            m_rotateAngle -= m_rotateSpeed * Time.deltaTime;
            if (m_rotateAngle <= -m_maxSideRotateAngle)
            {
                m_rotateAngle = -m_maxSideRotateAngle;
                m_isRotateRight = true;
            }
        }

        transform.rotation = Quaternion.AngleAxis(m_rotateAngle, Vector3.forward);
    }

    void MoveHook()
    {
        if (m_hookState != HookState.Launch)
        {
            return;
        }

        transform.position += transform.up * m_hookMoveSpeed * Time.deltaTime;
        if (Vector3.Distance(m_startPos, transform.position) >= m_maxHookFlyRange)
        {
            m_hookState = HookState.Retract;
        }
    }

    void RetractHook()
    {
        if (m_hookState != HookState.Retract)
        {
            return;
        }

        transform.position -= transform.up * m_hookMoveSpeed * Time.deltaTime;
        if (Vector3.Distance(m_startPos, transform.position) <= 0.1f)
        {
            if (m_currentCatchObject)
            {
                m_canCatch = true;
                Destroy(m_currentCatchObject);
                m_currentCatchObject = null;
                EventManager.TriggerEvent(EventName.CATCH, this.gameObject);
            }
            transform.position = m_startPos;
            m_hookState = HookState.Rotate;
        }
    }

    void OnTargetCatched(GameObject target)
    {
        if (!m_canCatch || m_hookState != HookState.Launch) return;
        m_canCatch = false;
        SetCatchableFollow(target);
        m_hookState = HookState.Retract;
    }

    void SetCatchableFollow(GameObject target)
    {
        target.transform.position = this.transform.position;
        target.transform.SetParent(this.transform);
        m_currentCatchObject = target;
        if (target.TryGetComponent<BounceTarget>(out var comp))
        {
            comp.OnCatch();
        }

        if (target.TryGetComponent<HiHiHiLogic>(out var comp1))
        {
            comp1.OnCatch();
        }
    }
}