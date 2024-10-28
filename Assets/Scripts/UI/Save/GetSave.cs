using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSave : MonoBehaviour
{
    public GameObject sItemPrefab; // 存档预设
    public Transform sListParent; // 存档列表的父对象

    public static int SelectNum;
    public Text selectText;
    public Text saveText;

    private int previousSelectNum; // 用于跟踪上一个选择的存档编号

    private void Start()
    {
        DisplaySaves();
        previousSelectNum = SelectNum; // 初始化上一个存档编号
    }

    private void Update()
    {
        // 监听 SelectNum 的变化
        if (SelectNum != previousSelectNum)
        {
            ShowSelect(); // 调用显示选择的存档文本
            previousSelectNum = SelectNum; // 更新上一个存档编号
        }
    }

    private void ShowSave()
    {
        if (SaveManager.NowNum > 0)
            saveText.text = "当前存档：" + SaveManager.NowNum.ToString();
    }

    private void ShowSelect()
    {
        if (SelectNum > 0)
            selectText.text = "点击切换为存档：" + SelectNum.ToString();
        else
            selectText.text = "当前未选中存档";
    }

    public void LoadSave()
    {
        SaveManager.Load(SelectNum);
        DisplaySaves();
    }

    private void DisplaySaves()
    {
        SelectNum = 0;
        ShowSelect();
        ShowSave();
        
        // 清空列表
        foreach (Transform child in sListParent)
        {
            Destroy(child.gameObject);
        }

        // 获取存档数量
        int saveCount = PlayerPrefs.GetInt(SaveManager.SaveCount, 0);

        for (int i = 1; i <= saveCount; i++) // 存档编号从1开始
        {
            // 获取存档数据
            string SDJson = PlayerPrefs.GetString(i.ToString(), "");
            if (!string.IsNullOrEmpty(SDJson))
            {
                // 反序列化存档数据
                SD saveData = JsonUtility.FromJson<SD>(SDJson);
                
                // 实例化存档项
                GameObject sItem = Instantiate(sItemPrefab, sListParent);
                sItem.GetComponent<SaveButton>().num = saveData.SDNum;

                // 获取 Text 组件
                Text text1 = sItem.transform.Find("Text1").GetComponent<Text>();
                Text text2 = sItem.transform.Find("Text2").GetComponent<Text>();
                Text text3 = sItem.transform.Find("Text3").GetComponent<Text>();
                int itemCount = GetTotalItemCount(saveData);
                // 设置文本内容
                text1.text = $"存档编号: {saveData.SDNum}";
                text2.text = $"锚点数: {CountTrueAnchors(saveData.Anchor)} 道具数: {itemCount}";
                text3.text = $"轮回：{saveData.Round}";
            }
        }
    }

    // 计算锚点中为true的数量
    public static int CountTrueAnchors(bool[] anchors)
    {
        int count = 0;
        foreach (bool anchor in anchors)
        {
            if (anchor) count++;
        }
        return count;
    }

    // 计算道具数量
    private int GetTotalItemCount(SD saveData)
    {
        int itemCount = 0;

        // 确保 EB 和 BB 列表存在并计算元素数量
        if (saveData.EB != null)
            itemCount += saveData.EB.Length;

        if (saveData.BB != null)
            itemCount += saveData.BB.Length;

        return itemCount;
    }

    public void BuildNewSave()
    {
        SaveManager.NewSave();
        DisplaySaves();
    }
}
