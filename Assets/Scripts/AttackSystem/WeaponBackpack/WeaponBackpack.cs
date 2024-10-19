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
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void DropBullet(Bullet BulletData)
    {
        if (EquippedBullets.Contains(BulletData)) { 
            EquippedBullets.Remove(BulletData);
            var drop = new GameObject("DroppedBullet");
            drop.AddComponent<SpriteRenderer>().sprite = BulletData.Icon;
            var col = drop.AddComponent<BoxCollider>();
            var rig = drop.AddComponent<Rigidbody>();
            col.isTrigger = false;
            col.excludeLayers = LayerIgnore_DroppedThings;
            float vel_x = UnityEngine.Random.Range(0f,1f);
            rig.useGravity = true;
            rig.velocity = new Vector3(vel_x, 1, 0).normalized * 3;
            rig.freezeRotation = false;
            rig.drag = 1;
        }
    }

    public void StoreEquippedBullet(Bullet equippedBulletData,Action OnBulletDataUpdate)
    {
        if (EquippedBullets.Contains(equippedBulletData)){ 
            EquippedBullets.Remove(equippedBulletData);
            BulletInBackpack.Add(equippedBulletData);
            OnBulletDataUpdate?.Invoke();
        }
    }

    public void EquipBulletFromBackpack(Bullet Bullet,Action OnBulletDataUpdate)
    {
        if (BulletInBackpack.Contains(Bullet)) { 
            BulletInBackpack.Remove(Bullet);
            EquippedBullets.Add(Bullet);
            OnBulletDataUpdate?.Invoke();
        }
    }
}
