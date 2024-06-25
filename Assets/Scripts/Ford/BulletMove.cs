using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletMove : MonoBehaviour
{
    private float _bulletSpeed = 20.0f;

    private bool _isDestroyed;

    private IObjectPool<BulletMove> _pool;

    private void OnEnable()
    {
        _isDestroyed = false;
        CancelInvoke("DestroyBullet");
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        Invoke("DestroyBullet", 5f);
    }

    public void SetManagedPool(IObjectPool<BulletMove> pool)
    { 
        _pool = pool;
    }

    public void DestroyBullet()
    {
        if (_isDestroyed) return;
        _isDestroyed = true;
        _pool.Release(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other}");
        if (other.gameObject.CompareTag("Enemy"))
        { 
            //todo ÇÇÇØ        
        }
        DestroyBullet();
    }
}
