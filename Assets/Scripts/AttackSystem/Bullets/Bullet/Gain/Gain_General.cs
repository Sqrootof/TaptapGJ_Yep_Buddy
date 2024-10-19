using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGain_General", menuName = "Data/Bullet/Gain/Gain_General", order = 0)]
public class Gain_General : Gain
{
    public new GainType GainType{
        get => GainType.General;
    }

    [SerializeField] float Gain_InitVelocity = 1;
    [SerializeField] float Gain_OffsetAngle = 0;
    [SerializeField] float Gain_Damage = 1;
    [SerializeField] float Gain_LifeTime = 1;


    public override void DeployGain(Projectile Projectile)
    {
        Projectile.InitialVelocity *= Gain_InitVelocity;
        Projectile.OffsetAngle += Gain_OffsetAngle;
        Projectile.Damage *= Gain_Damage;
        Projectile.LifeTime *= Gain_LifeTime;
    }
}
