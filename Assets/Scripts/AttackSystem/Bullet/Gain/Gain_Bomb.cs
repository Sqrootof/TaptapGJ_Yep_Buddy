using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGain_Bomb", menuName = "Data/Bullet/Gain/Gain_Bomb", order = 2)]
public class Gain_Bomb : Gain
{
    public new GainType GainType{
        get => GainType.Bomb;
    }

    [SerializeField] float Gain_BombRadius = 1;

    public override void DeployGain(Projectile Projectile)
    {
        Bomb newBomb = Projectile as Bomb;
        if (newBomb){
            newBomb.BombRadius *= Gain_BombRadius;
        }
    }
}
