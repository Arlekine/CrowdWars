using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public Action onTriggered;
    
    public void OnTriggerEnter(Collider other)
    {
        var soldier = other.GetComponent<Soldier>();

        if (soldier != null)
        {
            gameObject.SetActive(false);
            onTriggered?.Invoke();
        }
    }
}
