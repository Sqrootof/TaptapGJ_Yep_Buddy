using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMissile", menuName = "Data/Bullet/Projectile/Missile", order = 0)]
public class Missile : Projectile
{
    public new ProjectileType ProjectileType{
        get => ProjectileType.Missile;
    }

    [Space]
    public ParticleSystem Tail;
}
