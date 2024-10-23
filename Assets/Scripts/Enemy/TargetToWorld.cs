using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetToWorld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 在游戏开始时将此物体脱离其父物体
        DetachFromParent();
    }

    void DetachFromParent()
    {
        // 将此物体的父物体设置为 null
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        // 如果你需要在更新时进行其他操作，可以在这里添加代码
    }
}