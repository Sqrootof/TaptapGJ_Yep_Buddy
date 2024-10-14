using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHandler : ProjectileHandler
{
    [Header("×é¼þ")]
    Rigidbody Rigidbody;


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

    protected override void ComponentInit()
    {
        base.ComponentInit();
    }
}
