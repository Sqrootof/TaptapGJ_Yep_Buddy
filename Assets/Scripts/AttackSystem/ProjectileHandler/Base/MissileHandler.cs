using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHandler : ProjectileHandler
{
    [Header("组件")]
    Rigidbody Rigidbody;
    Collider Collider;
    ParticleSystem Particle;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) {
            //造成伤害
            if (OnProjectileHit != null) StartCoroutine(OnProjectileHit());
            DestroyProjectile();
        }
        else if (collision.gameObject.CompareTag("Obstacle")) { 
            if(OnProjectileHit != null) StartCoroutine (OnProjectileHit());
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        
    }

    public MissileHandler(Projectile Projectile) : base(Projectile){
        
    }

    public override void BeShoot(Vector3 StartPos,Vector3 MousePos)
    {
        base.BeShoot(StartPos, MousePos);
        ComponentInit();
        StartPos.z = 0;
        MousePos.z = 0;
        Vector3 Pointdir = (MousePos - StartPos).normalized;
        Pointdir.z = 0;
        transform.position = StartPos;

        float angle = 180 - Vector3.Angle(Vector3.up, Pointdir);
        if (Pointdir.x < 0){
            angle = -angle;    
        }
        
        float angleoffset = Random.Range(-ProjectileData.OffsetAngle, ProjectileData.OffsetAngle);
        angle += angleoffset;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Quaternion rotate = Quaternion.AngleAxis(angleoffset,Vector3.forward);
        Vector3 realDir = rotate * Pointdir;
        Rigidbody.velocity = realDir * ProjectileData.InitialVelocity;
    }

    protected override void ComponentInit()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        Particle = GetComponent<ParticleSystem>();
    }
}
