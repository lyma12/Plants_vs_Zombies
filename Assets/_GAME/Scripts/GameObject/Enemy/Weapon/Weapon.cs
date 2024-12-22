using System;
using UnityEngine;
public class Weapon : GameUnit
{
    [SerializeField] private Bullet bulletPrefab;

    public override void OnDespawn()
    {
        
    }

    public override void OnInit()
    {
        
    }
    public virtual void OnShoot(Action<Enemy> onHitVictim, Vector3 direction, Vector3 endPoint){
        if(bulletPrefab != null){
            Bullet bullet = SimplePool.Spawn<Bullet>(bulletPrefab, transform.position, Quaternion.identity);
            bullet.SetUp(onHitVictim, direction, endPoint);
        }
    }
}