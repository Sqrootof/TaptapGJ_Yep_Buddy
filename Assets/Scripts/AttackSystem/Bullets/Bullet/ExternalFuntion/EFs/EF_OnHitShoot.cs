using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EF_OnHitShoot",menuName ="Data/ExternalFunction/OnHitShoot",order = 0)]
public class EF_OnHitShoot : ExternalFunction
{
    [SerializeField]Projectile ProjectileToShootWhenHit;
    Rigidbody Rigidbody;
    [SerializeField]LayerMask HitLayer;

    public override void LoadExternalFuncDepend(List<Bullet> CBB,int CI,bool whetherLooped)
    {
        base.LoadExternalFuncDepend(CBB,CI,whetherLooped);
        Rigidbody = AttachTo.gameObject.GetComponent<Rigidbody>();
        //获取下一个飞弹
        if (!whetherLooped){
            do{
                if (CurrentIndex == CurrentBulletBlock.Count-1){
                    CurrentIndex = 0;
                    hasLooped = true;
                }
                else CurrentIndex++;
            } while (CurrentBulletBlock[CurrentIndex].BulletType != BulletType.Projectile);
            ProjectileToShootWhenHit = CurrentBulletBlock[CurrentIndex] as Projectile;
        }
        else ProjectileToShootWhenHit = null;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        AttachTo.OnProjectileHit += ExternalFunc;
    }

    public override IEnumerator ExternalFunc()
    {
        if (ProjectileToShootWhenHit){
            Projectile newData = ProjectileToShootWhenHit.DeepCopy() as Projectile;
            GameObject newbullet = Instantiate(ProjectileToShootWhenHit.Prefab);
            ProjectileHandler Handler = newbullet.GetComponent<ProjectileHandler>();
            //触发发射弹的EF没给AttachTo赋值
            if(newData.ExternalFunction) newData.ExternalFunction.AttachTo = Handler;
            newData.ExternalFunction?.LoadExternalFuncDepend(CurrentBulletBlock, CurrentIndex, hasLooped);
            newData.ProjectileHandler = Handler;
            Handler.SetProjectileData(newData);

            //反射
            //获取原方向
            Vector3 CurrentDir = Vector3.down; 
            Vector3 RotateAngle = AttachTo.transform.rotation.eulerAngles;
            Quaternion rotate = Quaternion.AngleAxis(RotateAngle.z,Vector3.forward);
            CurrentDir = rotate * CurrentDir;
            Debug.Log(CurrentDir);
            Physics.Raycast(AttachTo.transform.position, CurrentDir, out RaycastHit hit, 1, HitLayer);
            Vector3 NewVelocity = Vector3.Reflect(CurrentDir, hit.normal);
            Handler.BeShoot(AttachTo.transform.position + NewVelocity * 0.4f, AttachTo.transform.position + NewVelocity + NewVelocity);
            
            yield return null;
        }
        else yield return null;
    }

    public override ExternalFunction DeepCopy()
    {
        ExternalFunction newFunc = base.DeepCopy();
        (newFunc as EF_OnHitShoot).HitLayer = HitLayer;
        return newFunc;
    }
}
