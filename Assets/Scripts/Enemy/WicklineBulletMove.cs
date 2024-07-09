using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WicklineBulletMove : MonoBehaviour
{
    private Vector3 _direction;

    private float _bulletSpeed = 30.0f;
    private float _bullletDamage;

    private bool _isDestroyed;
    
    private IObjectPool<WicklineBulletMove> _pool;

    private void OnEnable()
    {
        _isDestroyed = false;
        CancelInvoke("DestroyBullet");
    }
    private void Update()
    {
        transform.Translate(_direction * _bulletSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        _direction = Vector3.forward;
        Invoke("DestroyBullet", 5f);
    }

    public void SetManagedPool(IObjectPool<WicklineBulletMove> pool)
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
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PlayerHurt(_bullletDamage);    
        }
        DestroyBullet();
    }
}
