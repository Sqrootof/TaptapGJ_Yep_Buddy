using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TestEF",menuName ="Data/Bullet/ExternalFuntion/TestEF",order =0)]
public class TestEF : ExternalFunction
{
    public GameObject der;

    public override void OnAwake()
    {
        base.OnAwake();
        AttachTo.OnProjectileFly += ExternalFunc;
    }

    public override IEnumerator ExternalFunc()
    {
        yield return base.ExternalFunc();
    }
}
