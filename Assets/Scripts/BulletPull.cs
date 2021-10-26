using System.Collections.Generic;
using UnityEngine;

public class BulletPull : MonoBehaviour
{
    public static BulletPull Inst;

    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private PullObject _bloodSplashPrefab;

    [SerializeField] private int _bulletStartAmount;
    [SerializeField] private int _bloodSplashStartAmount;
    
    [SerializeField] private float _bulletLifetime = 1.5f;
    [SerializeField] private float _bloodSplashLifetime = 1.5f;

    private List<Bullet> _freeBullets = new List<Bullet>();
    private List<PullObject> _freeBloodSplashes= new List<PullObject>();

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
        
        CreateBullets(_bulletStartAmount);
        CreateBlood(_bloodSplashStartAmount);
    }

    public Bullet GetBullet(Vector3 position)
    {
        if (_freeBullets.Count == 0)
            CreateBullets(5);

        var bullet = _freeBullets[0];
        _freeBullets.Remove(bullet);
        
        bullet.gameObject.SetActive(true);
        bullet.transform.position = position;
        bullet.Init(_bulletLifetime);

        bullet.onComplete += (b) =>
        {
            b.gameObject.SetActive(false);
            
            _freeBullets.Add(b);

            b.onComplete = null;
        };
        
        return bullet;
    }
    
    public GameObject GetBloodSplash(Vector3 position)
    {
        if (_freeBloodSplashes.Count == 0)
            CreateBlood(5);

        var bullet = _freeBloodSplashes[0];
        _freeBloodSplashes.Remove(bullet);
        
        bullet.gameObject.SetActive(true);
        bullet.transform.position = position;
        bullet.Init(_bloodSplashLifetime);

        bullet.onComplete += (b) =>
        {
            b.gameObject.SetActive(false);
            
            _freeBloodSplashes.Add(b);

            b.onComplete = null;
        };
        
        return bullet.gameObject;
    }

    public void CreateBullets(int bulletsToCreate)
    {
        for (int i = 0; i < bulletsToCreate; i++)
        {
            var newBullet = Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity);
            newBullet.gameObject.SetActive(false);
            _freeBullets.Add(newBullet);
        }
    }
    
    public void CreateBlood(int bulletsToCreate)
    {
        for (int i = 0; i < bulletsToCreate; i++)
        {
            var newFX = Instantiate(_bloodSplashPrefab, Vector3.zero, Quaternion.identity);
            newFX.gameObject.SetActive(false);
            _freeBloodSplashes.Add(newFX);
        }
    }
}