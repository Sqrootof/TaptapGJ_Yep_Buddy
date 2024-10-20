using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Pool;

public class ShootController : TIntance<ShootController>
{
    #region"射击相关"
    [SerializeField]List<Projectile> CurrentProjectileBlock = new();//下一个要射击的子弹块
    [SerializeField]List<Gain> CurrentGainsBlock = new();//下一个要搭载的增益块
    float Cooldown = 0;//射击冷却时间
    [SerializeField]float LastShootTime = -1;
    [SerializeField]public int BlockHeadIndex = 0;//下一个子弹块的头部索引
    public Camera Camera;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) {
            if (Time.time - LastShootTime >= Cooldown) {
                Shoot();
                LastShootTime = Time.time;
            } 
        }
    }

    public void Shoot()
    { 
        GetNextBulletBlock();
        Cooldown = 0;
        foreach (var Projectile in CurrentProjectileBlock)
        {
            Projectile newData = Projectile.DeepCopy() as Projectile;
            Cooldown += newData.CoolDown;
            foreach (var gain in CurrentGainsBlock)
            {
                gain.DeployGain(newData);
            }
            GameObject newbullet = Instantiate(Projectile.Prefab);
            ProjectileHandler Handler = newbullet.GetComponent<ProjectileHandler>();
            newData.ProjectileHandler = Handler;
            Handler.SetProjectileData(newData);
            Handler.BeShoot(transform.position, GetMousePosition());
        }
    }

    public Vector3 GetMousePosition()
    {
        // 获取鼠标屏幕位置
        Vector3 mouseScreenPosition = Input.mousePosition;
        float distanceFromCamera = -Camera.transform.position.z;
        mouseScreenPosition.z = distanceFromCamera;
        Vector3 mouseWorldPosition = Camera.ScreenToWorldPoint(mouseScreenPosition);
        return mouseWorldPosition;
    }

    /// <summary>
    /// 获取下一个子弹块
    /// </summary>
    /// <returns></returns>
    void  GetNextBulletBlock()
    {
        int Index = BlockHeadIndex++;
        if(BlockHeadIndex == WeaponBackpack.Instance.GetEquippedBullets().Count) 
            BlockHeadIndex = 0;

        CurrentProjectileBlock.Clear();
        CurrentGainsBlock.Clear();
        int stepcount = 1;
        int totalStep = 0;
        Bullet CurrentBullet;
        while (stepcount > 0)
        {
            stepcount--;
            CurrentBullet = WeaponBackpack.Instance.GetEquippedBullets()[Index++];
            if (Index == WeaponBackpack.Instance.GetEquippedBullets().Count)
                Index = 0;
            if (totalStep >= WeaponBackpack.Instance.GetEquippedBullets().Count)
                return;

            Cooldown += CurrentBullet.CoolDown;
            totalStep++;
            switch (CurrentBullet.BulletType)
            {
                case BulletType.Extend:
                    stepcount += (CurrentBullet as Extend).StepExtension;
                    continue;

                case BulletType.Gain:
                    stepcount++;
                    CurrentGainsBlock.Add(CurrentBullet as Gain);
                    continue;

                case BulletType.Projectile:
                    CurrentProjectileBlock.Add(CurrentBullet as Projectile);
                    continue;

                default:
                    Debug.LogError("Undefined Bullet Type");
                    break;
            }
        }

        if (Cooldown <= 0) Cooldown = Time.fixedDeltaTime; 
    }
}
