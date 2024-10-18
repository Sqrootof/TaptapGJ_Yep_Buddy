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
    public float InitialVelocity;//��ʼ�ٶ�
    public float OffsetAngle;//ƫ�ƽǶ�
    public float Damage;//�˺�
    public float LifeTime;//��������
    public bool SelfDamage;//�Ƿ���Լ�����˺�
    public GameObject Prefab;//Ԥ����
    public ExternalFunction ExternalFunction;

    /// <summary>
    /// ��̬���ɵ��ӵ�
    /// </summary>
    public ProjectileHandler ProjectileHandler;

    public Projectile DeepCopy(){ 
        Projectile target = Instantiate(this);
        if(ExternalFunction) target.ExternalFunction = ExternalFunction.DeepCopy();
        return target;
    }
}
