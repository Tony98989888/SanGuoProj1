using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Launcher : MonoBehaviour
{
    [SerializeField]
    private GameObject m_bounceTarget;
    private Animator m_animator;
    [SerializeField] private float m_minThrowInterval = 3;
    [SerializeField] private float m_maxThrowInterval = 5;

    [SerializeField] private Transform m_launcherTargetInitPos;

    private BounceTarget m_currentBounceTarget;

    private Vector3 m_initPos;
    
    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_initPos = transform.position;
    }

    private void OnEnable()
    {
        GameStartComponent.OnGameStart += OnGameStart;
        EventManager.StartListening(EventName.ON_GRAB_AREA_ENTER, OnGrabAreaEnter);
    }

    private void OnDisable()
    {
        GameStartComponent.OnGameStart -= OnGameStart;
        EventManager.StopListening(EventName.ON_GRAB_AREA_ENTER, OnGrabAreaEnter);
    }

    public void LauncherAnimationFinished()
    {
        var bounceTarget = GameObject.Instantiate(m_bounceTarget, m_launcherTargetInitPos.position, m_launcherTargetInitPos.rotation);
        m_currentBounceTarget = bounceTarget.GetComponent<BounceTarget>();
        EventManager.TriggerEvent(EventName.LAUNCHER_ANIMATION_FINISHED, this.gameObject);
    }

    IEnumerator LauncherThrow()
    {
        while (GameManager.Instance.CurrentGameState != GameState.OVER)
        {
            yield return new WaitForSeconds(Random.Range(m_minThrowInterval, m_maxThrowInterval));
            m_animator.SetTrigger("Throw");
        }
    }

    void OnGameStart()
    {
        // StartCoroutine(LauncherThrow());
        m_animator.SetTrigger("Throw");
    }

    void OnThrowEnd()
    {
        this.transform.position = m_initPos;
    }

    void OnGrabAreaEnter(GameObject target)
    {
        if (!m_currentBounceTarget.IsBounced)
        {
            return;
        }
        this.transform.position = target.transform.position;
        Destroy(target.gameObject);
        m_animator.SetTrigger("Throw");
    }
}
