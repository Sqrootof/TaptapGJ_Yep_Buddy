using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHandler : ProjectileHandler
{
    [Header("×é¼þ")]
    [SerializeField]Rigidbody Rigidbody;
    ParticleSystem ExplosionParticle;

    bool Exploded = false;

    private void Awake()
    {
        base.Awake();
    }

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

    public BombHandler(Projectile Projectile) : base(Projectile){
    
    }

    public override void BeShoot(Vector3 StartPos, Vector3 MousePos)
    {
        base.BeShoot(StartPos, MousePos);
        if ((ProjectileData as Bomb).useGravity){
            Rigidbody.useGravity = true;
        }
        StartPos.z = 0;
        MousePos.z = 0;

        Vector3 Pointdir = (MousePos - StartPos).normalized;
        transform.position = StartPos;

        float angle = 180 - Vector3.Angle(Vector3.up, Pointdir);
        if (Pointdir.x < 0)
        {
            angle = -angle;
        }

        float angleoffset = UnityEngine.Random.Range(-ProjectileData.OffsetAngle, ProjectileData.OffsetAngle);
        angle += angleoffset;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Quaternion rotate = Quaternion.AngleAxis(angleoffset, Vector3.forward);
        Vector3 realDir = rotate * Pointdir;
        Rigidbody.velocity = realDir * ProjectileData.InitialVelocity;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacles") || collision.gameObject.CompareTag("Shield") && !Exploded) {
            
            var en = Physics.OverlapSphere(transform.position,(ProjectileData as Bomb).BombRadius,LayerMask.GetMask("Enemy"));
            List<GameObject> col = new List<GameObject>();
            foreach (var co in en){
                if (!col.Contains(co.gameObject)) {
                    col.Add(co.gameObject);
                    Enemy ene = co.GetComponent<Enemy>();
                    if (ene) ene.currentHealth -= ProjectileData.Damage;
                } 
            }
            Exploded = true;
            HitCoroutine = InvokeProjectileLifeEvent(OnProjectileHit);
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Obstacles") || other.gameObject.CompareTag("Shield") && !Exploded){

            var en = Physics.OverlapSphere(transform.position, (ProjectileData as Bomb).BombRadius, LayerMask.GetMask("Enemy"));
            List<GameObject> col = new List<GameObject>();
            foreach (var co in en){
                if (!col.Contains(co.gameObject)){
                    col.Add(co.gameObject);
                    Enemy ene = co.GetComponent<Enemy>();
                    if (ene) ene.currentHealth -= ProjectileData.Damage;
                }
            }
            Exploded = true;
            HitCoroutine = InvokeProjectileLifeEvent(OnProjectileHit);
            DestroyProjectile();
        }
    }

    protected override void ComponentInit()
    {
        base.ComponentInit();
        Rigidbody = GetComponent<Rigidbody>();
        ExplosionParticle = GetComponent<ParticleSystem>();
        OnProjectileDestroy += PlayExplosionParticle;
    }

    IEnumerator PlayExplosionParticle() { 
        ExplosionParticle.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(ExplosionParticle.main.duration + ExplosionParticle.main.startLifetime.constantMax);
        DestroyCoroutine = null;
    }
}
