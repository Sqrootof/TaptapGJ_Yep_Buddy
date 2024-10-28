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
        if (SaveManager.NowSD != null)
        {
            var a = SaveManager.NowSD.EB;
            var b = SaveManager.NowSD.BB;
            SetEquippedBullets(a);
            SetBulletsInBackpack(b);
        }
    }
    /*private static Bullet CreateBullet(BulletData data)
    {
        // 在这里根据 BulletData 创建 Bullet 实例
        Bullet bullet = ScriptableObject.CreateInstance<Bullet>();
        bullet.BulletName = data.BulletName;
        bullet.Icon = data.Icon;
        bullet.BulletType = data.BulletType;
        bullet.ShootInterval = data.ShootInterval;
        return bullet;
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEquippedBullets(string[] addresses)
    {
        EquippedBullets = new List<Bullet>();
        foreach (var address in addresses)
        {
            Bullet bullet = Resources.Load<Bullet>(address);
            if (bullet != null)
            {
                EquippedBullets.Add(bullet);
            }
        }
    }

    public void SetBulletsInBackpack(string[] addresses)
    {
        BulletInBackpack = new List<Bullet>();
        foreach (var address in addresses)
        {
            Bullet bullet = Resources.Load<Bullet>(address);
            if (bullet != null)
            {
                BulletInBackpack.Add(bullet);
            }
        }
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

    public void EquipBulletFromBackpack(Bullet Bullet,Action OnBulletDataUpdate,int Index)
    {
        Debug.Log("Index:" + Index);
        Index = Index > EquippedBullets.Count-1 ? EquippedBullets.Count: Index;
        Debug.Log("__Index:" + Index);
        if (BulletInBackpack.Contains(Bullet)) {
            EquippedBullets.Insert(Index, Bullet);
            BulletInBackpack.Remove(Bullet);
        }
        else if (EquippedBullets.Contains(Bullet))
        {
            int CurrentIndex = EquippedBullets.IndexOf(Bullet);
            Debug.Log("CurrentIndex" + CurrentIndex);
            if (Index != CurrentIndex) { 
                if (Index < CurrentIndex) CurrentIndex += 1;
                else Index += 1;
                EquippedBullets.Insert(Index, Bullet);
                EquippedBullets.RemoveAt(CurrentIndex); 
            }
        }
        else Debug.LogError("意外的子弹装载来源");
        ShootController.Instance.BlockHeadIndex = 0;
        OnBulletDataUpdate?.Invoke();   
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
