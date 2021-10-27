using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public Action onTriggered;

    [Range(0,1)][SerializeField] private float _triggerAmount = 0.5f;
    
    private HashSet<Soldier> _triggerdSoldiers = new HashSet<Soldier>();

    private bool _isTriggered;
    
    public void OnTriggerEnter(Collider other)
    {
        var soldier = other.GetComponent<Soldier>();

        if (soldier != null)
        {
            _triggerdSoldiers.Add(soldier);
            StopAllCoroutines();
            StartCoroutine(CheckTrigger(soldier));
        }
    }

    private IEnumerator CheckTrigger(Soldier soldier)
    {
        yield return null;
        
        if (((float) _triggerdSoldiers.Count / (float) soldier.Squad.SquadCount) >= _triggerAmount)
        {
            gameObject.SetActive(false);

            if (!_isTriggered)
            {
                _isTriggered = true;
                onTriggered?.Invoke();
            }
        }
    }
}
