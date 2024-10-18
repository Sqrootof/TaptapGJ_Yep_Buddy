using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于处理射弹的公共生命周期
public class ProjectileHandler : MonoBehaviour
{
    [SerializeField]
    protected Projectile ProjectileData;//动态绑定的Projectile数据
    float AwakeTime;

    public delegate IEnumerator ProjectileLifeEvent();
    public ProjectileLifeEvent OnProjectileAwake;
    /// <summary>
    /// 此事件在射弹销毁时触发，第一行必须为yield return new WaitForEndOfFrame();或其他非空的yield return语句
    /// 最后一行必须为DestroyCoroutine = null;
    /// </summary>
    public ProjectileLifeEvent OnProjectileDestroy;
    public ProjectileLifeEvent OnProjectileHit;
    public ProjectileLifeEvent OnProjectileFly;
    protected Coroutine FlyCortine;
    protected Coroutine DestroyCoroutine;
    protected Coroutine HitCoroutine;

    public void Awake()
    {
        AwakeTime = Time.time;
        ComponentInit();
    }

    // Start is called before the first frame update
    public void Start()
    {
        LoadExternalFuntion();
        InvokeProjectileLifeEvent(OnProjectileAwake);
        FlyCortine = InvokeProjectileLifeEvent(OnProjectileFly);
    }

    public virtual void LoadExternalFuntion()
    {
        if (ProjectileData.ExternalFunction != null){
            ProjectileData.ExternalFunction.AttachTo = this;
            ProjectileData.ExternalFunction.OnAwake();
        }
    }

    protected virtual void ComponentInit() { }

    // Update is called once per frame
    public void Update()
    {
        if (Time.time - AwakeTime >= ProjectileData.LifeTime)
            DestroyProjectile();
    }

    /// <summary>
    /// 调这个方法销毁射弹，不要直接Destroy
    /// </summary>
    public void DestroyProjectile()
    {
        if (OnProjectileDestroy != null)
            DestroyCoroutine = StartCoroutine(OnProjectileDestroy());
        else
            Destroy(gameObject);

        StartCoroutine(IEDestroy());
    }

    IEnumerator IEDestroy()
    {
        while (DestroyCoroutine!=null || HitCoroutine!=null){
            yield return null;
        }
        Destroy(gameObject);
        yield return new WaitForEndOfFrame();
    }

    public void SetProjectileData(Projectile projectile){ 
        ProjectileData = projectile;
    }

    public Coroutine InvokeProjectileLifeEvent(ProjectileLifeEvent lifeEvent) {
        if (lifeEvent != null)
            return StartCoroutine(lifeEvent());
        return null;
    }

    private void OnDestroy()
    {
        if (FlyCortine != null) {
            StopCoroutine(FlyCortine);
        }
        OnProjectileAwake = null;
        OnProjectileDestroy = null;
        OnProjectileHit = null;
        OnProjectileFly = null;
    }

    public ProjectileHandler(Projectile projectile){ 
        ProjectileData = projectile;
    }

    public virtual void BeShoot(Vector3 StartPos, Vector3 MousePos) { }
}
