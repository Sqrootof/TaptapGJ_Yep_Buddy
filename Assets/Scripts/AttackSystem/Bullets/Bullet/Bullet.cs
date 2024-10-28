using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Projectile,//普通射弹
    Extend,//扩展射弹
    Gain//射弹增益
}

[Serializable]
public class Bullet : ScriptableObject
{
    public string BulletName;
    public Sprite Icon;
    public BulletType BulletType;
    public float ShootInterval;

    public virtual Bullet DeepCopy() { return Instantiate(this); }
}
