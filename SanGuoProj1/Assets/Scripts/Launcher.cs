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
    
    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameStartComponent.OnGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        GameStartComponent.OnGameStart -= OnGameStart;
    }

    public void LauncherAnimationFinished()
    {
        var bounceTarget = GameObject.Instantiate(m_bounceTarget, m_launcherTargetInitPos.position, m_launcherTargetInitPos.rotation);
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
}
