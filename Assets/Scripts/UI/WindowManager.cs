using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static bool isFullScreen = true;
    public GameObject window;
    public GameObject full;
    private const float targetAspect = 16f / 9f;
    
    // 设置为全屏模式
    public void SetFullscreen()
    {
        isFullScreen = true;
        FullOrWindow();
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

        // 获取当前屏幕的分辨率
        int currentWidth = Screen.currentResolution.width;
        int currentHeight = Screen.currentResolution.height;

        // 计算当前宽高比
        float currentAspect = (float)currentWidth / currentHeight;

        // 如果当前宽高比大于目标宽高比，调整高度
        if (currentAspect > targetAspect)
        {
            int newHeight = currentHeight;
            int newWidth = Mathf.FloorToInt(newHeight * targetAspect);
            Screen.SetResolution(newWidth, newHeight, true);
        }
        else // 否则，调整宽度
        {
            int newWidth = currentWidth;
            int newHeight = Mathf.FloorToInt(newWidth / targetAspect);
            Screen.SetResolution(newWidth, newHeight, true);
        }
    }

    // 设置为窗口模式
    public void SetWindowed()
    {
        isFullScreen = false;
        FullOrWindow();
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.SetResolution(1280, 720, false); // 设置窗口分辨率为1280x720
    }

    public void FullOrWindow()
    {
        if (isFullScreen)
        {
            full.SetActive(true); window.SetActive(false);
        }
        else
        {
            full.SetActive(false); window.SetActive(true);
        }
    }
}
