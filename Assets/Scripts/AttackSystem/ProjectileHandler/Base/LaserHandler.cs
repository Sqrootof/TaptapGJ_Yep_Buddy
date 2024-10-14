using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHandler : ProjectileHandler
{
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

    public LaserHandler(Projectile Projectile) : base(Projectile){
        
    }

    public override void BeShoot(Vector3 StartPos, Vector3 MousePos)
    {
        base.BeShoot(StartPos, MousePos);
    }

    protected override void ComponentInit()
    {
        base.ComponentInit();
    }
}
