﻿using UnityEngine;
   
   public class SquadFollower : MonoBehaviour
   {
       public bool IsFinal;

       [SerializeField] private LevelEndTrigger _LevelEndTrigger;
       [SerializeField] private SoldiersSquad _squad;
       [SerializeField] private Transform _camera;
       [SerializeField] private float _cameraSpeed;
       [Range(10, 100)] [SerializeField] private float _distanceA;
       [Range(10, 100)] [SerializeField] private float _distanceB;
       
       [Space] 
       [SerializeField] private Vector3 _lastArevaOffset;
       [SerializeField] private Vector3 _lastArenaRotation;

       private Vector3 _positionOffsetDirection;
       private Vector3 _targetPos;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    private void Start()
       {
           _positionOffsetDirection = _camera.transform.position - _squad.SquadCenter;
           _positionOffsetDirection = _positionOffsetDirection.normalized;
           _LevelEndTrigger.onTriggered += () => { IsFinal = true; };
       }
   
       private void FixedUpdate()
       {
           var squadCenter = _squad.SquadCenter;
           float distance = Mathf.Lerp(_distanceA, _distanceB, IsFinal ? 1f : ((float)_squad.SquadCount - 30f)/ 330f);

           if (squadCenter != Vector3.negativeInfinity)
           {
               _targetPos = _squad.SquadCenter + ( !IsFinal ? _positionOffsetDirection : _lastArevaOffset.normalized) * distance;

               if (IsFinal)
               {
                   var rotDifference = _lastArenaRotation - _camera.transform.localEulerAngles;
                   if (rotDifference.magnitude <= _cameraSpeed * Time.deltaTime)
                       _camera.transform.position = _targetPos;
                   else
                       _camera.transform.localEulerAngles = Vector3.Lerp(_camera.transform.localEulerAngles, _lastArenaRotation, (_cameraSpeed * Time.deltaTime));
               }

               var difference = _targetPos - _camera.transform.position;

               if (difference.magnitude <= _cameraSpeed * Time.deltaTime)
                   _camera.transform.position = _targetPos;
               else
                   _camera.transform.position = Vector3.Lerp(_camera.transform.position, _targetPos, (_cameraSpeed * Time.deltaTime));
           }
       }
   }