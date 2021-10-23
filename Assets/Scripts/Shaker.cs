using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public static Shaker Inst;

    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private Vector3 _strength = Vector3.one;
    [SerializeField] private int _frequency = 100;

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
    }

    public void Shake()
    {
        transform.DOShakePosition(_duration, _strength, _frequency);
    }
}
