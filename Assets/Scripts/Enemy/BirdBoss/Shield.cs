using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float timer=1f; // 旋转周期（秒）
    private float minRotation = -15f; // 最小旋转角度
    private float maxRotation = 75f;   // 最大旋转角度
    private float currentRotation;      // 当前旋转角度
    private bool rotatingToMax = true; // 当前是否在旋转到最大值

    // Update is called once per frame
    void Update()
    {
        // 计算当前的旋转速度
        float rotationSpeed = (maxRotation - minRotation) / timer * Time.deltaTime;

        // 根据当前状态更新旋转角度
        if (rotatingToMax)
        {
            currentRotation += rotationSpeed;
            if (currentRotation >= maxRotation) // 达到最大旋转角度
            {
                currentRotation = maxRotation; // 确保不超过最大值
                rotatingToMax = false; // 切换到旋转回最小值
            }
        }
        else
        {
            currentRotation -= rotationSpeed;
            if (currentRotation <= minRotation) // 达到最小旋转角度
            {
                currentRotation = minRotation; // 确保不低于最小值
                rotatingToMax = true; // 切换到旋转回最大值
            }
        }

        // 应用旋转到物体的Z轴
        transform.rotation = Quaternion.Euler(0, 0, currentRotation);
    }
}
