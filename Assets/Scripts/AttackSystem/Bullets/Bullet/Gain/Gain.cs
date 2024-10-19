using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GainType
{ 
    General,
    Laser,
    Bomb
}

public class Gain : Bullet
{
    public new BulletType BulletType{
        get => BulletType.Gain;
    }
    public GainType GainType;

    public virtual void DeployGain(Projectile Projectile) { }
}
