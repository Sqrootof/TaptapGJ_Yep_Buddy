using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Projectile,//∆’Õ®…‰µØ
    Extend,//¿©’π…‰µØ
    Gain//…‰µØ‘ˆ“Ê
}

public class Bullet : ScriptableObject
{
    public string BulletName;
    public Sprite Icon;
    public BulletType BulletType;
    public float CoolDown;

    public virtual Bullet DeepCopy() { return Instantiate(this); }
}
