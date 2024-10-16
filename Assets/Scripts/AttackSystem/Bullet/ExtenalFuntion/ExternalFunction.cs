using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalFunction : ScriptableObject
{
    [HideInInspector]
    public ProjectileHandler AttachTo;//¶¯Ì¬°ó¶¨µÄProjectileHandler

    public virtual void OnAwake() { }

    public virtual IEnumerator ExternalFunc() { yield return null; }

    public virtual ExternalFunction DeepCopy() {
        ExternalFunction instance = Instantiate(this);
        return instance;
    }
}
