using UnityEngine;
   
   public class SquadFollower : MonoBehaviour
   {
       public bool IsFinal;

       [SerializeField] private LevelEndTrigger _LevelEndTrigger;
       [SerializeField] private SoldiersSquad _squad;
       [SerializeField] private Transform _camera;
       [SerializeField] private float _cameraSpeed;
       [Range(10, 100)] [SerializeField] private float _distanceA;
       [Range(10, 100)] [SerializeField] private float _distanceB;

       private Vector3 _positionOffsetDirection;
       private Vector3 _targetPos;
   
       private void Start()
       {
           _positionOffsetDirection = _camera.transform.position - _squad.SquadCenter;
           _positionOffsetDirection = _positionOffsetDirection.normalized;
           _LevelEndTrigger.onTriggered += () => { IsFinal = true; };
       }
   
       private void Update()
       {
           var squadCenter = _squad.SquadCenter;
           float distance = Mathf.Lerp(_distanceA, _distanceB, IsFinal ? 1f : ((float)_squad.SquadCount - 30f)/ 330f);

           if (squadCenter != Vector3.negativeInfinity)
           {
               _targetPos = _squad.SquadCenter + _positionOffsetDirection * distance;
               var difference = _targetPos - _camera.transform.position;

               if (difference.magnitude <= _cameraSpeed * Time.deltaTime)
                   _camera.transform.position = _targetPos;
               else
                   _camera.transform.position = Vector3.Lerp(_camera.transform.position, _targetPos, (_cameraSpeed * Time.deltaTime));
           }
       }
   }