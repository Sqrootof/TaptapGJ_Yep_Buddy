using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBackpack : TIntance<WeaponBackpack>
{
    [SerializeField] List<Bullet> EquippedBullets = new();
    [SerializeField] List<Bullet> BulletInBackpack = new();

    [Header("掉落的武器装备相关")]
    [SerializeField] LayerMask LayerIgnore_DroppedThings;

    // Start is called before the first frame update
    void Start()
    {
        //SetEquippedBullets(SaveManager.NowSD.EB);
        //SetEquippedBullets(SaveManager.NowSD.BB);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEquippedBullets(List<Bullet> Bullets)
    {
        EquippedBullets = new List<Bullet>(Bullets);
    }

    public void SetBulletsInBackpack(List<Bullet> Bullets)
    {
        BulletInBackpack = new List<Bullet>(Bullets);
    }

    public List<Bullet> GetEquippedBullets() { 
        return EquippedBullets;
    }

    public List<Bullet> GetBulletInBackpack() { 
        return BulletInBackpack;
    }

    public void GetNewBullet(Bullet newBulletData)
    { 
        BulletInBackpack.Add(newBulletData.DeepCopy());
    }

    public void StoreEquippedBullet(Bullet equippedBulletData,Action OnBulletDataUpdate)
    {
        if (EquippedBullets.Contains(equippedBulletData)){ 
            EquippedBullets.Remove(equippedBulletData);
            BulletInBackpack.Add(equippedBulletData);
            ShootController.Instance.BlockHeadIndex = 0;
            OnBulletDataUpdate?.Invoke();
        }
    }

    public void EquipBulletFromBackpack(Bullet Bullet,Action OnBulletDataUpdate)
    {
        if (BulletInBackpack.Contains(Bullet)) { 
            BulletInBackpack.Remove(Bullet);
            EquippedBullets.Add(Bullet);
            ShootController.Instance.BlockHeadIndex = 0;
            OnBulletDataUpdate?.Invoke();
        }
    }

    public void DropBullet(Bullet bullet)
    {
        if (EquippedBullets.Contains(bullet))
        {
            EquippedBullets.Remove(bullet);
        }
        else if (BulletInBackpack.Contains(bullet))
        {
            BulletInBackpack.Remove(bullet);
        }
        else Debug.Log("子弹不存在");
    }
}
