using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGain_Laser", menuName = "Data/Bullet/Gain/Gain_Laser", order = 1)]
public class Gain_Laser : Gain
{
    public new GainType GainType {
        get => GainType.Laser;
    }
    [SerializeField]float Gain_LaserLength = 1;
    [SerializeField]float Gain_LaserWidth = 1;

    public override void DeployGain(Projectile Projectile)
    {
        Laser currentLaser = Projectile as Laser;
        if (currentLaser){ 
            currentLaser.LaserLength *= Gain_LaserLength;
            currentLaser.LaserWidth *= Gain_LaserWidth;
        }
    }
}
