using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EF_ClockShoot",menuName = "Data/ExternalFunction/ClockShoot",order =1)]
public class EF_ClockShoot : ExternalFunction
{
    [SerializeField] float Time;
    [SerializeField] Projectile ProjtileToShootOnTime;

    public override void OnAwake()
    {
        base.OnAwake();
        AttachTo.OnProjectileFly += ExternalFunc;
    }

    public override ExternalFunction DeepCopy()
    {
        return base.DeepCopy();
    }

    public override IEnumerator ExternalFunc()
    {
        if (ProjtileToShootOnTime){
            yield return new WaitForSeconds(Time);
            Projectile newData = ProjtileToShootOnTime.DeepCopy() as Projectile;
            GameObject newbullet = Instantiate(ProjtileToShootOnTime.Prefab);
            ProjectileHandler Handler = newbullet.GetComponent<ProjectileHandler>();
            //触发发射弹的EF没给AttachTo赋值
            if (newData.ExternalFunction) newData.ExternalFunction.AttachTo = Handler;
            newData.ExternalFunction?.LoadExternalFuncDepend(CurrentBulletBlock, CurrentIndex, hasLooped);
            newData.ProjectileHandler = Handler;
            Handler.SetProjectileData(newData);

            Vector3 CurrentDir = Vector3.down;
            Vector3 RotateAngle = AttachTo.transform.rotation.eulerAngles;
            Quaternion rotate = Quaternion.AngleAxis(RotateAngle.z, Vector3.forward);
            CurrentDir = rotate * CurrentDir;

            Handler.BeShoot(AttachTo.transform.position, AttachTo.transform.position+CurrentDir);
        }
    }

    public override void LoadExternalFuncDepend(List<Bullet> CBB, int CI, bool whetherLooped)
    {
        base.LoadExternalFuncDepend(CBB, CI, whetherLooped);
        //获取下一个飞弹
        if (!whetherLooped)
        {
            do
            {
                if (CurrentIndex == CurrentBulletBlock.Count - 1)
                {
                    CurrentIndex = 0;
                    hasLooped = true;
                }
                else CurrentIndex++;
            } while (CurrentBulletBlock[CurrentIndex].BulletType != BulletType.Projectile);
            ProjtileToShootOnTime = CurrentBulletBlock[CurrentIndex] as Projectile;
        }
        else ProjtileToShootOnTime = null;
    }
    
}
