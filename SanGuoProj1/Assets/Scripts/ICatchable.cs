using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICatchable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Hook"))
        {
            EventManager.TriggerEvent(EventName.TARGET_CATCHED, this.gameObject);
        }
    }
}
