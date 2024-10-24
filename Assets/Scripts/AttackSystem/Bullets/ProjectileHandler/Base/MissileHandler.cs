using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHandler : ProjectileHandler
{
    [Header("×é¼þ")]
    Rigidbody Rigidbody;
    ParticleSystem Particle;

    private void Awake()
    {
        base.Awake();
        Particle = transform.GetComponentInChildren<ParticleSystem>();
        Particle.Play();
        if (OnProjectileFly != null) StartCoroutine(OnProjectileFly());
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    protected override void ComponentInit()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) {
            Debug.Log("Hit Enemy");
            collision.gameObject.GetComponent<Enemy>().currentHealth -= ProjectileData.Damage;
            HitCoroutine = InvokeProjectileLifeEvent(OnProjectileHit); 
            DestroyProjectile();
        }
        else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacles") || collision.gameObject.CompareTag("Shield")) {
            HitCoroutine = InvokeProjectileLifeEvent(OnProjectileHit);
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            trigger.gameObject.GetComponent<Enemy>().currentHealth -= ProjectileData.Damage;
            HitCoroutine = InvokeProjectileLifeEvent(OnProjectileHit);
            DestroyProjectile();
        }
        else if (trigger.gameObject.CompareTag("Ground") || trigger.gameObject.CompareTag("Obstacles") || trigger.gameObject.CompareTag("Shield"))
        {
            HitCoroutine = InvokeProjectileLifeEvent(OnProjectileHit);
            DestroyProjectile();
        }
    }

    public override void BeShoot(Vector3 StartPos,Vector3 MousePos)
    {
        base.BeShoot(StartPos, MousePos);
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

    public MissileHandler(Projectile Projectile) : base(Projectile)
    {

    }
}
