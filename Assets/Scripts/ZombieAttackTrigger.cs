using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackTrigger : MonoBehaviour
{
    [SerializeField] private Zombie _zombie;
    
    private void GiveDamage()
    {
        _zombie.GiveDamage();
    }
}
