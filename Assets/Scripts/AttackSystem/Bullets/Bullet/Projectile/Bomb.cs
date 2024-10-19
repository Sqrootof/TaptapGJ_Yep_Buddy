using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBomb", menuName = "Data/Bullet/Projectile/Bomb", order = 2)]
public class Bomb : Projectile
{
    public new ProjectileType ProjectileType{
        get => ProjectileType.Bomb;
    }

    [Space]
    public bool useGravity;
    public float BombRadius;
}
