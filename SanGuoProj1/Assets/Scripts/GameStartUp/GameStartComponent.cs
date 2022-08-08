using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartComponent : MonoBehaviour
{
    [SerializeField] private int m_countDownTime = 3;

    public static event Action OnGameStart;
    [SerializeField]
    private TMP_Text m_text;

    IEnumerator StartTimeCount()
    {
        for (int i = m_countDownTime; i >= 0; i--)
        {
            m_text.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
            if (i == 0)
            {
                m_text.text = "Start";
                yield return new WaitForSeconds(1.0f);
                OnGameStart?.Invoke();
                m_text.enabled = false;
                yield break;
            }
        }
    }

    private void Start()
    {
        StartCoroutine(StartTimeCount());
    }
}
