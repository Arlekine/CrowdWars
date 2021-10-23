using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class SoldiersIncreaser : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private SoldiersIncreaser _brotherGate;

    private void Awake()
    {
        SetText(_text);
    }

    public void OnTriggerEnter(Collider other)
    {
        var soldier = other.GetComponent<Soldier>();

        if (soldier != null)
        {
            IncreaseSquad(soldier.Squad);
            gameObject.SetActive(false);
            _brotherGate.DisableGate();
        }
    }

    public void DisableGate()
    {
        GetComponent<Collider>().enabled = false;
    }

    protected abstract void IncreaseSquad(SoldiersSquad squad);
    protected abstract void SetText(TextMeshProUGUI text);
}