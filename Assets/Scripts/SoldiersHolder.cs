using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldiersHolder : MonoBehaviour
{
    public static SoldiersHolder Inst;

    public List<Soldier> Soldiers => _squad.Soldiers;
    
    [SerializeField] private SoldiersSquad _squad;
    
    private void Awake()
    {
        if (Inst == null)
            Inst = this;
    }

    private void Update()
    {
        foreach (var soldier in Soldiers)
        {
            soldier.UpdateTarget(ZombieHolder.Inst.AllZombies);
        }
    }
}
