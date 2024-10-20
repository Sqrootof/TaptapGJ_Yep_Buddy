using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PictorialBookDisplay : MonoBehaviour
{
    public GameObject pictorialEntryPrefab; // 用于创建图鉴条目的Prefab
    public Transform contentPanel; // UI中显示条目的父对象

    private void Start()
    {
        DisplayPictorialBooks();
    }

    public void DisplayPictorialBooks()
    {
        //// 清空现有的UI元素
        //foreach (Transform child in contentPanel)
        //{
        //    Destroy(child.gameObject);
        //}

        // 从FillPictorialBook获取图鉴列表
        List<PictorialBook> pictorialBooks = Whole.pictorialBooks;
        foreach (var book in pictorialBooks)
        {
            if (book.entryName != "")
            {
                // 实例化图鉴条目
                GameObject entry = Instantiate(pictorialEntryPrefab, contentPanel);
                // 获取Image和Text组件
                Image entryImage = entry.transform.Find("Image").GetComponent<Image>();
                Text entryText = entry.transform.Find("Text").GetComponent<Text>();

                // 设置图鉴名称和图片
                if (book.isUnlocked)
                {
                    entryText.text = book.entryName;

                    // 创建Sprite并赋值给Image组件
                    entryImage.sprite = book.entryImage;
                }
                else
                {
                    entryText.text = "XXXX"; // 未解锁状态
                    entryImage.sprite = null; // 或者显示一个默认图像
                }
            }

        }
    }
}
