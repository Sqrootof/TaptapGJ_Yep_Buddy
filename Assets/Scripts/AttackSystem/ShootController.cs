using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Pool;

public class ShootController : MonoBehaviour
{
    #region"射击相关"
    [SerializeField]List<Bullet> Bullets = new();//当前搭载的子弹

    [SerializeField]List<Projectile> CurrentProjectileBlock = new();//下一个要射击的子弹块
    [SerializeField]List<Gain> CurrentGainsBlock = new();//下一个要搭载的增益块
    float Cooldown = 0;//射击冷却时间
    float LastShootTime = -1;
    [SerializeField]int BlockHeadIndex = 0;//下一个子弹块的头部索引
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (Time.time - LastShootTime >= Cooldown) {
                Shoot();
            } 
        }
    }

    public void Shoot()
    { 
        GetNextBulletBlock();
        Cooldown = 0;
        foreach (var Projectile in CurrentProjectileBlock)
        {
            Projectile newData = Instantiate(Projectile);
            Cooldown += newData.CoolDown;
            foreach (var gain in CurrentGainsBlock)
            {
                gain.DeployGain(newData);
            }
            GameObject newbullet = Instantiate(Projectile.Prefab);
            ProjectileHandler Handler = newbullet.GetComponent<ProjectileHandler>();
            Handler.SetProjectileData(newData);
            
            Handler.BeShoot(transform.position,Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    /// <summary>
    /// 获取下一个子弹块
    /// </summary>
    /// <returns></returns>
    void  GetNextBulletBlock()
    {
        int Index = BlockHeadIndex++;
        if(BlockHeadIndex == Bullets.Count) 
            BlockHeadIndex = 0;

        CurrentProjectileBlock.Clear();
        CurrentGainsBlock.Clear();
        int stepcount = 1;
        Bullet CurrentBullet;
        while (stepcount > 0)
        {
            stepcount--;
            CurrentBullet = Bullets[Index++];
            if (Index == Bullets.Count)
                Index = 0;
            if (CurrentProjectileBlock.Contains(CurrentBullet as Projectile) || CurrentGainsBlock.Contains(CurrentBullet as Gain)){
                break;
            }
            switch (CurrentBullet.BulletType)
            {
                case BulletType.Extend:
                    stepcount += (CurrentBullet as Extend).StepExtension;
                    break;

                case BulletType.Gain:
                    stepcount++;
                    CurrentGainsBlock.Add(CurrentBullet as Gain);
                    break;

                case BulletType.Projectile:
                    CurrentProjectileBlock.Add(CurrentBullet as Projectile);
                    break;

                default:
                    Debug.LogError("Undefined Bullet Type");
                    break;
            }
        }
    }
}
