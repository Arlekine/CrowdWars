using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Soldier : MonoBehaviour
{
    public Action<Soldier> onDead;
    
    public SimpleCharControl Mover => _charControl;

    public SoldiersSquad Squad;
    
    [SerializeField] private ParticleSystem _shotFX;
    [SerializeField] private int _damage = 4;
    [SerializeField] private float _attackDistance = 0.5f;
    [SerializeField] private float _attackPause = 0.5f;
    [SerializeField] private int _startHealth = 12;
    
    private Collider _collider;
    private Rigidbody _rigidbody;
    
    private Animator _animator;
    private SimpleCharControl _charControl;
    private int _health;
    private bool _isAttacking;

    private float _nextAttackTime;
    private float _nextUpdateTargetTime;
    
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

    public void UpdateTarget(List<Zombie> zombies)
    {
        bool isAttack = false;
        _charControl.SetRotation = true;
        
        float targetDistance;

        if (Time.time > _nextUpdateTargetTime)
        {
            _nextUpdateTargetTime = Time.time + Random.Range(0.05f, 0.15f);
            for (int i = 0; i < zombies.Count; i++)
            {
                targetDistance = (zombies[i].transform.position - transform.position).sqrMagnitude;
                if (targetDistance < _attackDistance * _attackDistance)
                {
                    _currentTarget = zombies[i];
                }
            }
        }

        if (_currentTarget != null)
        {
            Vector3 distanceToTarget = GetCurrentTargetDistance();

            if (distanceToTarget.sqrMagnitude < _attackDistance * _attackDistance)
            {
                isAttack = true;
                _charControl.SetRotation = false;

                if ((transform.forward - (distanceToTarget).normalized).sqrMagnitude > _lerpStep * _lerpStep)
                {
                    transform.forward =
                        Vector3.Lerp(transform.forward, (distanceToTarget).normalized, _lerpStep);
                }
                else
                {
                    transform.forward = (distanceToTarget).normalized;
                }

                if ((transform.forward - (distanceToTarget).normalized).sqrMagnitude < _lerpStep * _lerpStep * 4)
                {
                    if (Time.time >= _nextAttackTime)
                    {
                        _nextAttackTime = Time.time + _attackPause;
                        Shoot();
                    }
                }


                transform.rotation =
                    Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
            }
        }

        if (isAttack != _isAttacking)
        {
            _isAttacking = isAttack;
            _animator.SetBool("IsAttacking", _isAttacking);
        }
    }

    public void Shoot()
    {
        _shotFX.Play(true);
        var bullet = BulletPull.Inst.GetBullet(transform.position);
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