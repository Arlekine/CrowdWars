using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombieHolder : MonoBehaviour
{
    public static ZombieHolder Inst;

    public List<Zombie> AllZombies => _isFinal ? _lastZombies : _levelZombies;

    [SerializeField] private LevelEndTrigger _levelEndTrigger;
    [SerializeField] private Transform _levelZomdiesParent;
    [SerializeField] private Transform _finalZomdiesParent;

    private int _steps;
    private bool _isFinal;
    private List<Zombie> _levelZombies;
    private List<Zombie> _lastZombies;

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
        
        _levelZombies = _levelZomdiesParent.GetComponentsInChildren<Zombie>().ToList();
        _lastZombies = _finalZomdiesParent.GetComponentsInChildren<Zombie>().ToList();

        _levelEndTrigger.onTriggered += () =>
        {
            _isFinal = true;
            foreach (var zombie in _lastZombies)
            {
                zombie.SetActive(true);
            }
        };
        
        foreach (var zombie in _levelZombies)
        {
            zombie.onDead += (z) => { _levelZombies.Remove(z);};
            zombie.SetActive(true);
        }
        
        foreach (var zombie in _lastZombies)
        {
            zombie.onDead += (z) => { _lastZombies.Remove(z);};
            zombie.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < AllZombies.Count; i++)
        {
            AllZombies[i].UpdateTarget(SoldiersHolder.Inst.Soldiers);
        }
    }
}
