using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : MonoBehaviour
{
    private MeshRenderer objectRenderer; // 改为 MeshRenderer
    private Color startColor;
    public float flashSpeed;
    private float timeCounter = 0f;

    void Start()
    {
        objectRenderer = GetComponent<MeshRenderer>(); // 获取 MeshRenderer 组件
        startColor = objectRenderer.material.color; // 存储原始颜色
    }

    void Update()
    {
        timeCounter += Time.deltaTime * flashSpeed; // 增加时间计数
        float alpha = Mathf.PingPong(timeCounter, 1f); // 创建一个在 0 到 1 之间的值
        // 设置新颜色，修改 alpha 值
        objectRenderer.material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
    }
}
