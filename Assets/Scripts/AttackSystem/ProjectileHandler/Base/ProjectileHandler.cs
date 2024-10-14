using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于处理射弹的公共生命周期
public class ProjectileHandler : MonoBehaviour
{
    protected Projectile ProjectileData;

    float AwakeTime;

    public delegate IEnumerator ProjectileLifeEvent();
    public ProjectileLifeEvent OnPeojectileAwake;
    public ProjectileLifeEvent OnProjectileSleep;
    public ProjectileLifeEvent OnProjectileHit;
    public ProjectileLifeEvent OnProjectileFly;

    bool ReadyToDestroy = false;
    // Start is called before the first frame update
    public void Start()
    {
        AwakeTime = Time.time;
        ComponentInit();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Time.time - AwakeTime >= ProjectileData.LifeTime)
            DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        if (OnProjectileSleep != null)
            StartCoroutine(OnProjectileSleep());
        else
            Destroy(gameObject);

        StartCoroutine(IEDestroy());
    }

    IEnumerator IEDestroy()
    {
        while (!ReadyToDestroy){
            yield return null;
        }
        Destroy(gameObject);
    }

    public void SetProjectileData(Projectile projectile){ 
        ProjectileData = projectile;
    }

    public ProjectileHandler(Projectile projectile){ 
        ProjectileData = projectile;
    }

    public virtual void BeShoot(Vector3 StartPos, Vector3 MousePos) { }

    protected virtual void ComponentInit() { }
}
