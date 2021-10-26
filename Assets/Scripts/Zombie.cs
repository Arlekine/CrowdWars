using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Zombie : MonoBehaviour
{
    public Action<Zombie> onDead;
    
    [SerializeField] private SimpleCharControl _SimpleCharControl;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _enemyDetectionRadious = 10f;
    [SerializeField] private int _damage = 4;
    [SerializeField] private float _attackDistance = 0.5f;

    [SerializeField] private int _startHealth = 12;
    
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private Color _deathColor;
    [SerializeField] private SkinnedMeshRenderer _skinnedMesh;    
    private int _health;
    
    private Soldier _currentTarget;
    
    private float _nextUpdateTargetTime;
    private Collider[] colliers;

    private void Start()
    {
        _health = _startHealth;
    }

    public void UpdateTarget(List<Soldier> soldiers)
    {
        float targetDistance;

        if (Time.time > _nextUpdateTargetTime)
        {
            _nextUpdateTargetTime = Time.time + Random.Range(0.05f, 0.15f);;
            foreach (var soldier in soldiers)
            {
                targetDistance = (soldier.transform.position - transform.position).sqrMagnitude;
                if (targetDistance < _enemyDetectionRadious * _enemyDetectionRadious)
                {
                    if (_currentTarget == null ||
                        (_currentTarget.transform.position - transform.position).sqrMagnitude >
                        targetDistance)
                    {
                        _currentTarget = soldier;
                    }
                }
            }
        }


        if (_currentTarget != null)
        {
            Vector3 distanceToTarget = GetCurrentTargetDistance();
            _animator.SetBool("IsAttacking", false);

            if (distanceToTarget.sqrMagnitude > _attackDistance*_attackDistance)
            {
                _SimpleCharControl.Move((distanceToTarget).normalized);
            }
            else
            {
                _animator.SetBool("IsAttacking", true);
                
        
                if ((transform.forward - (distanceToTarget).normalized).sqrMagnitude > _lerpStep * _lerpStep)
                    transform.forward = Vector3.Lerp(transform.forward, (distanceToTarget).normalized, _lerpStep);
                else
                    transform.forward = (distanceToTarget).normalized;
                
                
                transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
            }
        }
        else
        {
            _animator.SetBool("IsAttacking", false);
        }
    }

    private float _lerpStep = 0.1f;

    private Vector3 GetCurrentTargetDistance()
    {
        if (_currentTarget == null)
            return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        else
        {
            return _currentTarget.transform.position - transform.position;
        }
    }

    public void GiveDamage()
    {
        if (_currentTarget != null)
        {
            _currentTarget.Hit(_damage);
        }
    }
    
    
    public void Hit(int damage)
    {
        _health -= damage;
        
        if (_health <= 0)
            Death();
    }

    private void Death()
    {
        _skinnedMesh.material.color = _deathColor;

        _animator.SetTrigger("Death");
        _SimpleCharControl._isDead = true;
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        onDead?.Invoke(this);
        Destroy(this);
    }

    public void SetActive(bool isActive)
    {
        _animator.enabled = isActive;
        _rigidbody.isKinematic = !isActive;
    }
}
