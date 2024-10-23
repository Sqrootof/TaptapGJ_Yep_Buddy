using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelChoice : MonoBehaviour
{
    public GameObject[] ui;
    public GameObject[] buttons;
    private Image[] images;

    public static int Num = 0;

    private void Awake()
    {
        // 初始化 images 数组，大小与 buttons 数组相同
        images = new Image[buttons.Length];

        // 循环遍历 buttons 数组
        for (int i = 0; i < buttons.Length; i++)
        {
            // 获取每个按钮的 Image 组件并赋值到 images 数组
            images[i] = buttons[i].GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        AltButton(Num);
        AltUI(Num);
    }

    public void AltButton(int a)
    {
        if (images.Length > 0)
        {
            foreach (var t in images)
            {
                t.color = Color.white;
            }
            images[a].color = Color.yellow;
        }
    }
    
    public void AltUI(int a)
    {
        if (ui.Length > 0)
        {
            foreach (var t in ui)
            {
                t.SetActive(false);
            }
            ui[a].SetActive(true);
        }
    }
}
