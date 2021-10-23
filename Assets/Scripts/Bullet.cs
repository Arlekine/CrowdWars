using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bloodEffect;
    
    private Vector3 _currentDirection;
    private float _deathTime;


    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }

    public void SetDirection(Vector3 direction)
    {
        _currentDirection = direction;
        _deathTime = Time.time + 15f;
    }

    private void Update()
    {
        transform.Translate(_currentDirection * 30f * Time.deltaTime, Space.Self);

        if (Time.time > _deathTime)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        var zombie = other.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombie.Hit(2);
            Instantiate(_bloodEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}