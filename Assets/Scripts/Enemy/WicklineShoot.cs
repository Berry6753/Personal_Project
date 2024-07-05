using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WicklineShoot : MonoBehaviour
{
    private IObjectPool<WicklineBulletMove> _bulletPool;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private List<GameObject> _shootPoints;

    private void Awake()
    {
        _bulletPool = new ObjectPool<WicklineBulletMove>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet);
    }

    private void OnAttack()
    {
        foreach (var spawn in _shootPoints)
        {
            var bullet = _bulletPool.Get();
            bullet.Shoot();
            bullet.transform.position = spawn.transform.position;
            bullet.transform.rotation = spawn.transform.rotation;
        }
    }

    private WicklineBulletMove CreateBullet()
    {
        WicklineBulletMove bullet = Instantiate(_bulletPrefab, transform).GetComponent<WicklineBulletMove>();
        bullet.SetManagedPool(_bulletPool);
        return bullet;
    }
    private void OnGetBullet(WicklineBulletMove bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    private void OnReleaseBullet(WicklineBulletMove bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void OnDestroyBullet(WicklineBulletMove bullet)
    {
        Destroy(bullet.gameObject);
    }
}
