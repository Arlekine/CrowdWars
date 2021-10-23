using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBlocker : MonoBehaviour
{
    [SerializeField] private SoldiersSquad _squad;
    [SerializeField] private Transform _finalArenaStart;

    private float _zOffset;

    private void Start()
    {
        _zOffset = transform.position.z - _squad.SquadCenter.z;
    }

    private void Update()
    {
        var pos = transform.position;
        pos.z = Mathf.Min(pos.z, _squad.SquadCenter.z + _zOffset);
        pos.z= Mathf.Max(pos.z, _finalArenaStart.position.z);

        transform.position = pos;
    }
}
