using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy1 : MonoBehaviour
{
    private void Awake()
    {
            DontDestroyOnLoad(gameObject); // 防止在场景切换时被销毁
    }
}
