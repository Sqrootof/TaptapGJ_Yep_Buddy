using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLaser", menuName = "Data/Bullet/Projectile/Laser", order = 1)]
public class Laser : Projectile
{
    public new ProjectileType ProjectileType{
        get => ProjectileType.Laser;
    }

    [Space]
    public float LaserWidth;
    public float LaserLength;
}
