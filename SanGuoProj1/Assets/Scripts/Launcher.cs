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
    
    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void LauncherAnimationFinished()
    {
        var bounceTarget = GameObject.Instantiate(m_bounceTarget, transform.position, transform.rotation);
        EventManager.TriggerEvent(EventName.LAUNCHER_ANIMATION_FINISHED, this.gameObject);
    }

    IEnumerator LauncherThrow()
    {
        yield return new WaitForSeconds(Random.Range(m_minThrowInterval, m_maxThrowInterval));
        m_animator.SetTrigger("Throw");
    }

    // private void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (col.CompareTag("Catchable"))
    //     {
    //         EventManager.TriggerEvent(EventName.TARGET_CATCHED, this.gameObject);
    //     }
    // }
}
