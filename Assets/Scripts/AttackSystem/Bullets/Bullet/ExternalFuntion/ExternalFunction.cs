using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalFunction : ScriptableObject
{
    public ProjectileHandler AttachTo;//¶¯Ì¬°ó¶¨µÄProjectileHandler
    public List<Bullet> CurrentBulletBlock;
    public string Description;
    [HideInInspector] public int CurrentIndex;
    [HideInInspector] public bool hasLooped = false; 
    public virtual void OnAwake() { }

    public virtual IEnumerator ExternalFunc() { yield return new WaitForEndOfFrame(); }

    public virtual ExternalFunction DeepCopy() {
        ExternalFunction instance = Instantiate(this);
        return instance;
    }

    public virtual void LoadExternalFuncDepend(List<Bullet> CBB,int CI,bool whetherLooped) {
        CurrentBulletBlock = CBB;
        CurrentIndex = CI;
    }
}
