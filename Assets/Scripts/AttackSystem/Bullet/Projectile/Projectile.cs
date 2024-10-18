using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ProjectileType
{ 
    Missile,
    Laser,
    Bomb
}

public class Projectile : Bullet
{
    public new BulletType BulletType { 
        get => BulletType.Projectile; }

    public ProjectileType ProjectileType;
    public float InitialVelocity;//初始速度
    public float OffsetAngle;//偏移角度
    public float Damage;//伤害
    public float LifeTime;//生命周期
    public bool SelfDamage;//是否对自己造成伤害
    public GameObject Prefab;//预制体
    public ExternalFunction ExternalFunction;

    /// <summary>
    /// 动态生成的子弹
    /// </summary>
    public ProjectileHandler ProjectileHandler;

    public Projectile DeepCopy(){ 
        Projectile target = Instantiate(this);
        if(ExternalFunction) target.ExternalFunction = ExternalFunction.DeepCopy();
        return target;
    }
}
