using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Action<Bullet> onComplete;
    [SerializeField] private TrailRenderer _trail;
    
    private Vector3 _currentDirection;

    public void Init(float lifeTime)
    {
        _trail.Clear();
        StartCoroutine(LifeRoutine(lifeTime));
    }

    private IEnumerator LifeRoutine(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        onComplete?.Invoke(this);
    }

    public void SetDirection(Vector3 direction)
    {
        _currentDirection = direction;
    }

    private void Update()
    {
        transform.Translate(_currentDirection * 30f * Time.deltaTime, Space.Self);
    }

    public void OnTriggerEnter(Collider other)
    {
        var zombie = other.GetComponent<Zombie>();

        if (other.tag == "Border")
        {
            StopAllCoroutines();
            onComplete?.Invoke(this);
        }

        if (zombie != null)
        {
            zombie.Hit(2);
            BulletPull.Inst.GetBloodSplash(transform.position);
            
            StopAllCoroutines();
            onComplete?.Invoke(this);
        }
    }
}