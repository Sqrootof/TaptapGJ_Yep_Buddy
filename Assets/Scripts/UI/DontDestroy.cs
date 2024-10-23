using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy instance; // 静态实例

    private void Awake()
    {
        // 检查是否已经存在实例
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 如果实例已经存在，销毁当前对象
        }
        else
        {
            instance = this; // 如果没有实例，设置当前实例
            DontDestroyOnLoad(gameObject); // 防止在场景切换时被销毁
        }
    }
}
