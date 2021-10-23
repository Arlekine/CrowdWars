using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private int _health;
    
    private Soldier _currentTarget;
    private Collider[] colliers;

    private void Start()
    {
        _health = _startHealth;
    }

    private void Update()
    {
        //if (_currentTarget == null || GetCurrentTargetDistance().magnitude > 10f)
        if (GetCurrentTargetDistance().magnitude > _attackDistance)
        {
            colliers = Physics.OverlapSphere(transform.position, _enemyDetectionRadious, _targetLayer);
            float currentTargetDistance = float.MaxValue;
            Soldier character = null;
            
            foreach (var collier in colliers)
            {
                character = collier.GetComponent<Soldier>();
                if (character != null)
                {
                    if (_currentTarget == null || (_currentTarget.transform.position - transform.position).magnitude > (character.transform.position - transform.position).magnitude)
                    {
                        _currentTarget = character;
                    }
                }
            }
        }

        if (_currentTarget != null)
        {
            Vector3 distanceToTarget = GetCurrentTargetDistance();
            _animator.SetBool("IsAttacking", false);

            if (distanceToTarget.magnitude > _attackDistance)
            {
                _SimpleCharControl.Move((distanceToTarget).normalized);
            }
            else
            {
                _animator.SetBool("IsAttacking", true);
                
        
                if ((transform.forward - (distanceToTarget).normalized).magnitude > _lerpStep)
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
        _animator.SetTrigger("Death");
        _SimpleCharControl._isDead = true;
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        onDead?.Invoke(this);
        Destroy(this);
    }
}
