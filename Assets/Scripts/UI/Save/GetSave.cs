using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSave : MonoBehaviour
{
    public GameObject sItemPrefab; // 预设
    public Transform sListParent; // 存档列表的父对象
    public GameObject loadButtonPrefab; // 加载按钮预设
    private string currentSaveName; // 当前选中的存档名
    public static int Select;
    public Text selectText;

    public Text text;

    private void Start()
    {
        Select = -1;
        RefreshSaveList(); // 初始显示存档列表
    }

    private void RefreshSaveList()
    {
        text.text = "当前存档：" + SaveManager.NowName;
        if (Select == -1)
            selectText.text = "未选中存档";
        else
            selectText.text = "点击启用存档：" + Select.ToString();
        // 清除现有的存档显示
        foreach (Transform child in sListParent)
        {
            Destroy(child.gameObject);
        }

        // 获取存档数量
        int saveCount = PlayerPrefs.GetInt("SaveCount", 0);
        
        // 遍历所有存档
        for (int i = 0; i < saveCount; i++)
        {
            if (PlayerPrefs.HasKey(i.ToString()))
            {
                // 获取存档的 JSON 数据
                string saveData = PlayerPrefs.GetString(i.ToString(), "");
                if (!string.IsNullOrEmpty(saveData))
                {
                    // 解析存档
                    SD saveDataObj = JsonUtility.FromJson<SD>(saveData);

                    // 实例化 UI 元素
                    GameObject item = Instantiate(sItemPrefab, sListParent);

                    // 获取显示文本组件并设置内容
                    Text itemText = item.GetComponentInChildren<Text>();
                    if (itemText != null)
                    {
                        itemText.text = "Save Name: " + saveDataObj.SDname + " | Save Num: " + saveDataObj.SDNum;
                    }


                    // 添加点击事件
                    Button itemButton = item.GetComponent<Button>();
                    if (itemButton != null)
                    {
                        itemButton.onClick.AddListener(() => SelectSave(saveDataObj.SDname.ToString()));
                    }
                }
            }
        }
    }

    public void CreateNewSave()
    {
        // 创建新的存档
        SaveManager.NewSave();
        
        // 刷新存档列表显示
        RefreshSaveList();
    }

    private void SelectSave(string saveName)
    {
        // 记录当前选中的存档名
        currentSaveName = saveName;

        // 显示加载按钮
        ShowLoadButton();
    }

    private void ShowLoadButton()
    {
        // 清除已有的加载按钮（如果存在）
        foreach (Transform child in sListParent)
        {
            if (child.name == "LoadButton")
            {
                Destroy(child.gameObject);
            }
        }

        // 实例化新的加载按钮
        GameObject loadButton = Instantiate(loadButtonPrefab, sListParent);
        loadButton.name = "LoadButton"; // 给按钮命名，方便后续查找

        // 获取按钮组件并添加事件
        Button button = loadButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => LoadSave(currentSaveName));
            Text buttonText = loadButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Load Save: " + currentSaveName;
            }
        }
    }

    public void LoadSave(string saveName)
    {
        // 加载指定的存档
        SaveManager.Load(saveName);
        // 这里可以添加加载成功后的逻辑，例如跳转到游戏主界面等
    }
}
