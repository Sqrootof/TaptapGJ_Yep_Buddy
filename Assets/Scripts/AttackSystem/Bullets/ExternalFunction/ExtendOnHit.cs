using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendOnHit : ExternalFunction
{
    public override void OnAwake()
    {
        base.OnAwake();
        AttachTo.OnProjectileHit += ExternalFunc;
    }

    public override IEnumerator ExternalFunc()
    {
        return base.ExternalFunc();

    }
}
