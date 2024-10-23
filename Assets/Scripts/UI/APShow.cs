using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class APShow : MonoBehaviour
{
    public GameObject anchorItemPrefab; // 预设
    public Transform anchorListParent; // 成就列表的父对象
    public AnchorManager player;

    void OnEnable()
    {
        DisplayAchievements();
    }

    void DisplayAchievements()
    {
        // 清空列表
        foreach (Transform child in anchorListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var t in player.anchorPoints)
        {
            GameObject anchorItem = Instantiate(anchorItemPrefab, anchorListParent);
            Image image = anchorItem.GetComponent<Image>();
            
            if (t.isUnlocked)
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
            }
        }
    }
}
