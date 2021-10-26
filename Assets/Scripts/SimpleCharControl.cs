using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleCharControl : MonoBehaviour
{
    public bool SetRotation;
    
    [SerializeField] private float _moveSpeed;

    private Rigidbody _CharacterController;
    private Animator _Animator;
    private Quaternion _initialRotation;

    private Vector3 _prevPosition;
    private float _lerpStep = 0.1f;
    public bool _isDead;
    public bool _isMoving;
    
    private void Start()
    {
        _CharacterController = GetComponent<Rigidbody>();
        _Animator = GetComponentInChildren<Animator>();
        _initialRotation = _CharacterController.rotation;
        _prevPosition = _CharacterController.position;
    }

    private void LateUpdate()
    {
        _CharacterController.rotation = Quaternion.Euler(new Vector3(0f, _CharacterController.rotation.eulerAngles.y, 0f));
        
        _CharacterController.velocity = Vector3.zero;
        _CharacterController.angularVelocity = Vector3.zero;
        
        if (_isMoving)
            _isMoving = false;
        else
            _Animator.SetBool("Move", false);
    }

    public void Move(Vector3 direction)
    {
        if (_isDead)
            return;

        if (!_isMoving)
        {
            _isMoving = true;
            _Animator.SetBool("Move", true);
        }

        if (SetRotation)
        {
            if ((transform.forward - direction).magnitude > _lerpStep)
                transform.forward = Vector3.Lerp(transform.forward, direction, _lerpStep);
            else
                transform.forward = direction;
        }

        _CharacterController.MovePosition(_CharacterController.position + direction * (_moveSpeed * Random.Range(0.95f, 1.05f) * Time.deltaTime));
    }
}
