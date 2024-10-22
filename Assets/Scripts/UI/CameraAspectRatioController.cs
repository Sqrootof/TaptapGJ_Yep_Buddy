using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraAspectRatioController : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        UpdateCameraViewport();
    }

    private void Update()
    {
        // 在Update中检测窗口大小的变化，并实时更新摄像机视口
        if (!WindowManager.isFullScreen || Screen.width != cam.pixelWidth || Screen.height != cam.pixelHeight)
        {
            UpdateCameraViewport();
        }
    }

    private void UpdateCameraViewport()
    {
        float targetAspect = 16f / 9f; // 目标宽高比

        float windowAspect = (float)Screen.width / Screen.height; // 当前窗口宽高比

        float scaleHeight = windowAspect / targetAspect; // 计算当前窗口宽高比与目标宽高比的比值

        UnityEngine.Rect rect = cam.rect;

        if (scaleHeight < 1.0f) // 如果窗口宽高比小于目标宽高比，意味着窗口过宽
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else // 如果窗口宽高比大于目标宽高比，意味着窗口过窄
        {
            float scaleWidth = 1.0f / scaleHeight; // 计算宽度比值
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        cam.rect = rect; // 更新摄像机视口
        cam.ResetAspect();
    }
}
