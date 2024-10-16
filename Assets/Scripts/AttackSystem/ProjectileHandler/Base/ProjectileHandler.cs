using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ڴ����䵯�Ĺ�����������
public class ProjectileHandler : MonoBehaviour
{
    [SerializeField]
    protected Projectile ProjectileData;//��̬�󶨵�Projectile����
    float AwakeTime;

    public delegate IEnumerator ProjectileLifeEvent();
    public ProjectileLifeEvent OnProjectileAwake;
    /// <summary>
    /// ���¼����䵯����ʱ��������һ�б���Ϊyield return new WaitForEndOfFrame();�������ǿյ�yield return���
    /// ���һ�б���Ϊyield return null;
    /// </summary>
    public ProjectileLifeEvent OnProjectileDestroy;
    public ProjectileLifeEvent OnProjectileHit;
    public ProjectileLifeEvent OnProjectileFly;
    private Coroutine FlyCortine;
    private Coroutine DestroyCoroutine;
    // Start is called before the first frame update
    public void Start()
    {
        AwakeTime = Time.time;
        ComponentInit();
        LoadExternalFuntion();
        InvokeProjectileLifeEvent(OnProjectileAwake);
        FlyCortine = InvokeProjectileLifeEvent(OnProjectileFly);
    }

    public virtual void LoadExternalFuntion()
    {
        ProjectileData.ExternalFunction.AttachTo = this;
        ProjectileData.ExternalFunction.OnAwake();
    }

    protected virtual void ComponentInit() { }

    // Update is called once per frame
    public void Update()
    {
        if (Time.time - AwakeTime >= ProjectileData.LifeTime)
            DestroyProjectile();
    }

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
        while (DestroyCoroutine != null){
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
