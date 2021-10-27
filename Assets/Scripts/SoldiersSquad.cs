using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoldiersSquad : MonoBehaviour
{
    public Action onSquadDestroyed;

    public List<Soldier> Soldiers => _chars;
    public Vector3 SquadCenter => GetSquadCenter();
    public int SquadCount => _chars.Count;

    [SerializeField] private float _horizontalControlSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float _screenMinDelta;
    [SerializeField] private LevelEndTrigger _levelEndTrigger;
    [SerializeField] private List<Soldier> _chars = new List<Soldier>();
    [SerializeField] private Soldier _soldierPrefab;

    private bool _isFinal;
    
    private void Awake()
    {
        foreach (var soldier in _chars)
        {
            soldier.onDead += RemoveSoldier;
            soldier.Squad = this;
        }

        _levelEndTrigger.onTriggered += () => { _isFinal = true;};
    }

    private void Update()
    {
        var fingers = LeanTouch.GetFingers(false, false);

        Vector3 moveDirection = new Vector3(0f, 0f, -moveSpeed * Time.deltaTime);

        if (_isFinal)
            moveDirection.z = 0f;
        
        if (fingers.Count > 0)
        {
            Vector2 fingerDelta;

            if (_isFinal)
                fingerDelta = fingers[0].ScreenPosition - fingers[0].StartScreenPosition;
            else
            {
                fingerDelta = fingers[0].ScreenDelta;
            }

            if(_isFinal)
                moveDirection = new Vector3(-Mathf.Sign(fingerDelta.x) * moveSpeed * Time.deltaTime, 0f, -Mathf.Sign(fingerDelta.y) * moveSpeed * Time.deltaTime);
            else
            {
                moveDirection.x = -(fingerDelta.normalized.x * _horizontalControlSpeed * Time.deltaTime);
            }
        }
        
        if (moveDirection.magnitude > float.Epsilon)
        {
            foreach (var charControl in _chars)
            {
                charControl.Mover.Move(moveDirection);
            }
        }
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

public class ZomdiesComparer : IComparer<Zombie>
{
    public int Compare(Zombie x, Zombie y)
    {
        if (x == null || y == null)
        {
            throw new ArgumentException("You can use TitleLastKeyNumberComparer only for Header elements");
        }
        
        return x.transform.position.z.CompareTo(y.transform.position.z);
    }
}