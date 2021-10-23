using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoldiersSquad : MonoBehaviour
{
    public Action onSquadDestroyed;
    
    public Vector3 SquadCenter => GetSquadCenter();
    public int SquadCount => _chars.Count;
    
    [SerializeField] private List<Soldier> _chars = new List<Soldier>();
    [SerializeField] private Soldier _soldierPrefab;

    private void Awake()
    {
        foreach (var soldier in _chars)
        {
            soldier.onDead += RemoveSoldier;
            soldier.Squad = this;
        }
    }

    private void Update()
    {
        var fingers = LeanTouch.GetFingers(false, false);

        if (fingers.Count > 0)
        {
            Vector2 fingerDelta = fingers[0].ScreenPosition - fingers[0].StartScreenPosition;
            Vector3 moveDirection = new Vector3(-fingerDelta.x, 0f, -fingerDelta.y);

            if (moveDirection.magnitude > float.Epsilon)
            {
                foreach (var charControl in _chars)
                {
                    charControl.Mover.Move(moveDirection.normalized);
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
            AddSoldiers(50);
    }
    
    public void AddSoldiers(int amount)
    {
        var squadCenter = GetSquadCenter();
        
        for (int i = 0; i < amount; i++)
        {
            var randomOffest = Random.insideUnitSphere * amount / 20;
            randomOffest.y = 0;
            var newSoldier = Instantiate(_soldierPrefab, squadCenter + randomOffest, Quaternion.identity);
            _chars.Add(newSoldier);
            newSoldier.onDead += RemoveSoldier;
            newSoldier.Squad = this;
        }
    }

    public void RemoveSoldiers(int amount)
    {
        print(amount);
        amount = Mathf.Min(amount, _chars.Count - 1);
        print(amount);
        
        _chars.Sort(new SoldierComparer());
        List<Soldier> charsToRemove = new List<Soldier>();
        
        
        for (int i = 0; i < amount; i++)
        {
            charsToRemove.Add(_chars[i]);
        }

        foreach (var soldier in charsToRemove)
        {
            soldier.Hit(100);
        }
    }

    private void RemoveSoldier(Soldier soldier)
    {
        _chars.Remove(soldier);

        if (_chars.Count == 0)
        {
            onSquadDestroyed?.Invoke();
        }
    }

    private Vector3 GetSquadCenter()
    {
        if (_chars.Count == 0)
            return Vector3.negativeInfinity;;
        
        var totalX = 0f;
        var totalY = 0f;
        var totalZ = 0f;
        
        foreach(var player in _chars)
        {
            totalX += player.transform.position.x;
            totalY += player.transform.position.y;
            totalZ += player.transform.position.z;
        }
        
        var centerX = totalX / _chars.Count;
        var centerY = totalY / _chars.Count;
        var centerZ = totalZ / _chars.Count;
        
        return new Vector3(centerX, centerY, centerZ);
    }
}

public class SoldierComparer : IComparer<Soldier>
{
    public int Compare(Soldier x, Soldier y)
    {
        if (x == null || y == null)
        {
            throw new ArgumentException("You can use TitleLastKeyNumberComparer only for Header elements");
        }
        
        return y.transform.position.z.CompareTo(x.transform.position.z);
    }
}