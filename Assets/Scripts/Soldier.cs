using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Action<Soldier> onDead;
    
    public SimpleCharControl Mover => _charControl;

    public SoldiersSquad Squad;
    
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private ParticleSystem _shotFX;
    [SerializeField] private int _damage = 4;
    [SerializeField] private float _attackDistance = 0.5f;
    [SerializeField] private float _attackPause = 0.5f;
    [SerializeField] private int _startHealth = 12;
    [SerializeField] private LayerMask _targetLayer;
    
    private Collider _collider;
    private Rigidbody _rigidbody;
    
    private Animator _animator;
    private SimpleCharControl _charControl;
    private int _health;

    private float _nextAttackTime;
    
    private void Start()
    {
        _health = _startHealth;
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _charControl = GetComponentInChildren<SimpleCharControl>();
    }
    
    private Zombie _currentTarget;
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
    
    private void Update()
    {
        _animator.SetBool("IsAttacking", false);
        _charControl.SetRotation = true;
        
        //if (_currentTarget == null || GetCurrentTargetDistance().magnitude > 10f)
        if (GetCurrentTargetDistance().magnitude > _attackDistance)
        {
            var colliers = Physics.OverlapSphere(transform.position, 10f, _targetLayer);
            float currentTargetDistance = float.MaxValue;
            
            foreach (var collier in colliers)
            {
                var character = collier.GetComponent<Zombie>();
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

            if (distanceToTarget.magnitude < _attackDistance)
            {
                _animator.SetBool("IsAttacking", true);
                _charControl.SetRotation = false;

                if (Time.time >= _nextAttackTime)
                {
                    _nextAttackTime = Time.time + _attackPause;
                    Shoot();
                }


                if ((transform.forward - (distanceToTarget).normalized).magnitude > _lerpStep)
                    transform.forward =
                        Vector3.Lerp(transform.forward, (distanceToTarget).normalized, _lerpStep);
                else
                    transform.forward = (distanceToTarget).normalized;


                transform.rotation =
                    Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
            }
        }
    }

    public void Shoot()
    {
        _shotFX.Play(true);
        var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.SetDirection(transform.forward);
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
        _charControl._isDead = true;
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        onDead?.Invoke(this);
        Destroy(this);
    }
}