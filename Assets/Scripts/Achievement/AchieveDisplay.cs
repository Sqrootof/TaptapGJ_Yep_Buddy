using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementDisplay : MonoBehaviour
{
    public GameObject achievementItemPrefab; // 预设
    public Transform achievementListParent; // 成就列表的父对象

    void Start()
    {
        DisplayAchievements();
    }

    void DisplayAchievements()
    {
        // 清空成就列表
        foreach (Transform child in achievementListParent)
        {
            Destroy(child.gameObject);
        }

        // 循环遍历成就列表并显示
        foreach (Achievement achievement in Whole.achievements)
        {
            GameObject achievementItem = Instantiate(achievementItemPrefab, achievementListParent);
            Image itemImage = achievementItem.GetComponent<Image>();
            Text[] texts = achievementItem.GetComponentsInChildren<Text>();

            // 设置成就名称
            texts[0].text = achievement.name +":"+ achievement.conditions;

            if (achievement.isUnlocked)
            {
                // 使用 Color.white
                itemImage.color = new Color(1f, 1f, 1f, 1f); // Image 颜色
                texts[0].color = new Color(0f, 0f, 0f, 1f); // 文本颜色改为黑色，不透明
            }
            else
            {
                // 可以选择设置成就未解锁的颜色，比如灰色
                itemImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Image 颜色
                texts[0].color = new Color(0.5f, 0.5f, 0.5f,0.5f); // 设置为灰色
            }
        }
    }
}
