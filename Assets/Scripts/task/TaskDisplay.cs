using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskDisplay : MonoBehaviour
{
    public GameObject tItemPrefab; // 预设
    public Transform tListParent; // 成就列表的父对象

    void OnEnable()
    {
        DisplayAchievements();
    }

    void DisplayAchievements()
    {
        // 清空列表
        foreach (Transform child in tListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var t in Whole.questList)
        {
            GameObject item = Instantiate(tItemPrefab, tListParent);
            Image image = item.GetComponent<Image>();
            Text text1 = item.transform.Find("Text1").GetComponent<Text>();
            if (text1 != null)
            {
                // 设置文本内容为 AnchorPointName
                text1.text = t.taskName;
            }

            Text text2 = item.transform.Find("Text2").GetComponent<Text>();
            if (text1 != null)
            {
                text2.text = t.description[t.Process];
            }
            
            /*if (t.)
            {
                image.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                image.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            
            Text textComponent = anchorItem.transform.Find("Text").GetComponent<Text>();
            if (textComponent != null)
            {
                // 设置文本内容为 AnchorPointName
                textComponent.text = t.AnchorPointName;
            }
            else
            {
                Debug.LogWarning("Text component not found in " + anchorItem.name);
            }*/
        }
    }
}
