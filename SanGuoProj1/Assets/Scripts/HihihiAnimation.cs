using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HihihiAnimation : MonoBehaviour
{
    private bool m_isCatchable = true;
    
    public void EnableCatchable()
    {
        m_isCatchable = true;
    }

    public void DisableCatchable()
    {
        m_isCatchable = false;
    }
}
