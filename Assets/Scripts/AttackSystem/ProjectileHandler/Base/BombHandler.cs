using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHandler : ProjectileHandler
{
    [Header("组件")]
    Rigidbody Rigidbody;
    ParticleSystem ExplosionParticle;
    Explosion Explosion;

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
        if ((ProjectileData as Bomb).useGravity) {
            Rigidbody.useGravity = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) {
            GameObject newBomb = transform.GetChild(0).gameObject;
            newBomb.SetActive(true);
            Explosion explosion  = newBomb.AddComponent<Explosion>();
            newBomb.GetComponent<SphereCollider>().radius = (ProjectileData as Bomb).BombRadius;
            explosion.DestroyBomb += DestroyProjectile;
            GetComponent<SpriteRenderer>().enabled = false;
            ExplosionParticle?.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    protected override void ComponentInit()
    {
        base.ComponentInit();
        Rigidbody = GetComponent<Rigidbody>();
        ExplosionParticle = GetComponent<ParticleSystem>();
        ExplosionParticle.loop = false;
    }
}

public class Explosion : MonoBehaviour
{
    public event Action DestroyBomb;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) { 
            //造成伤害
            DestroyBomb?.Invoke();
        }
    }
}
