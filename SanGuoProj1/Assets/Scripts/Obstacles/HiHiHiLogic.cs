using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiHiHiLogic : ICatchable
{
    private BoxCollider2D m_boxCollider;

    private void Start()
    {
        m_boxCollider = GetComponent<BoxCollider2D>();
    }

    public void OnCatch()
    {
        m_boxCollider.enabled = false;
    }
}
