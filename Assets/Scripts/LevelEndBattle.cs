using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelEndBattle : MonoBehaviour
{
    public Action onWin;
    
    [SerializeField] private GameObject _zombiesToKillParent;

    private List<Zombie> _zombiesToKill;
    
    private void Start()
    {
        _zombiesToKill = new List<Zombie>();
        _zombiesToKill = _zombiesToKillParent.GetComponentsInChildren<Zombie>(true).ToList();

        foreach (var zombie in _zombiesToKill)
        {
            zombie.onDead += RemoveZomdieFromList;
        }
    }

    private void RemoveZomdieFromList(Zombie zomdie)
    {
        _zombiesToKill.Remove(zomdie);

        if (_zombiesToKill.Count == 0)
        {
            print("Win!");
            onWin?.Invoke();
        }
    }
}
