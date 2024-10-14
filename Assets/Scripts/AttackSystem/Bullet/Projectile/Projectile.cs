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
    public float CoolDown;//充能时间
    public bool SelfDamage;//是否对自己造成伤害
    public GameObject Prefab;//预制体
    
    ProjectileHandler projectileHandler = null;
    public ProjectileHandler ProjectileHandler
    {
        get { 
            if(projectileHandler)
                return projectileHandler;
            else
                projectileHandler = Prefab.GetComponent<ProjectileHandler>();
            return projectileHandler;
        }
    }
}
