using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPYP : MonoBehaviour
{
    private float timer = 0f;  // 用于计时
    public float destroyTime = 1f;  // 设定摧毁的时间（1秒）

    // Update is called once per frame
    void Update()
    {
        // 更新计时器
        timer += Time.deltaTime;

        // 检查计时器是否超过设定时间
        if (timer >= destroyTime)
        {
            // 摧毁当前物体
            Destroy(gameObject);
        }
    }
}
