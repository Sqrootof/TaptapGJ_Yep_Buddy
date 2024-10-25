using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EF_ClockShoot",menuName = "Data/ExternalFunction/ClockShoot",order =1)]
public class EF_ClockShoot : ExternalFunction
{
    [SerializeField] float Time;
    Projectile ProjtileToShootOnTime;

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
        yield return new WaitForSeconds(Time);
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
