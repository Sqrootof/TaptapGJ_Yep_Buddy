using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间

public class BlackScreen : MonoBehaviour
{
    private Image image; // 将 SpriteRenderer 替换为 Image

    public bool fadeInOut = false; // 控制渐变的公共变量
    private bool fadingIn = true; // 是否在渐变到不透明
    private bool fading = false; // 是否正在进行渐变过程

    public float stayTime; // 停留时间
    public float fadeSpeed = 1.0f; // 渐变速度
    private float currentAlpha = 0f; // 当前透明度
    private float time;

    private void Start()
    {
        image = GetComponent<Image>(); // 获取 Image 组件
        image.color = new Color(0, 0, 0, 0); // 初始化为完全透明
    }

    private void Update()
    {
        // 当公共变量变为 true 且不在进行渐变时，开始渐变过程
        if (fadeInOut && !fading)
        {
            fading = true; // 开始渐变
            currentAlpha = 0f; // 重置当前透明度
            fadingIn = true; // 设置为从透明到不透明
        }

        // 渐变过程
        if (fading && time <= 0f)
        {
            // 修改透明度
            currentAlpha = Mathf.MoveTowards(currentAlpha, fadingIn ? 1f : 0f, fadeSpeed * Time.deltaTime);
            image.color = new Color(0, 0, 0, currentAlpha); // 更新 Image 组件的颜色

            // 检查是否达到了目标透明度
            if (Mathf.Approximately(currentAlpha, fadingIn ? 1f : 0f)) // 使用 Mathf.Approximately 检查浮点数相等
            {
                // 如果已经完成渐变，停止渐变
                if (fadingIn)
                {
                    fadingIn = false; // 反转为渐变回透明
                    time = stayTime; // 设置停留时间
                }
                else
                {
                    fading = false; // 完成整个渐变过程
                    fadeInOut = false; // 重新设置控制变量为 false
                    gameObject.SetActive(false);
                }
            }
        }
        if (time >= 0f)
        {
            time -= Time.deltaTime; // 减少计时器
        }
    }
}
