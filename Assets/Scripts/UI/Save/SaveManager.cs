using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SD
{
    public int SDNum; // 存档编号
    public bool[] Anchor; // 锚点状态
    public string[] EB; // 装备子弹列表
    public string[] BB; // 背包子弹列表
}

/*[Serializable]
public class BulletData
{
    public string BulletName;
    public Sprite Icon;
    public BulletType BulletType;
    public float ShootInterval;
}*/

public class SaveManager : MonoBehaviour
{
    public static SD NowSD;
    public static int NowNum;
    public const string LastUsed = "LastUsed";
    public const string SaveCount = "SaveCount";
    public static bool IsLoggedIn = false;
    private int lastUsedSave;
    
    //加载目标存档：SaveManager.Load(int name);
    //保存当前存档：SaveManager.Save(SaveManager.NowNum);

    private void Start()
    {
        lastUsedSave = PlayerPrefs.GetInt(LastUsed, 0);
        if (lastUsedSave > 0)
        {
            Load(lastUsedSave);
        }
        else
        {
            NewSave();
        }
    }

    //加载，由num获取NowSD、NowNum，改变LastUsed。
    public static void Load(int num)
    {
        string SDJson = PlayerPrefs.GetString(num.ToString(), "");
        if (!string.IsNullOrEmpty(SDJson))
        {
            NowSD = JsonUtility.FromJson<SD>(SDJson);
            NowNum = NowSD.SDNum;
            
            IsLoggedIn = true;
            PlayerPrefs.SetInt(LastUsed,NowNum); PlayerPrefs.Save();
        }
        Debug.Log("Loading: " + SDJson);
    }

    //保存，获取bool[]和两个List，修改NowSD并保存。
    public static void Save(int num)
    {
        if (Whole.anchorPoints.Count > 1)
        {
            NowSD.BB = WeaponBackpack.Instance.GetBulletInBackpack().Select(b => b.Address).ToArray();
            NowSD.EB = WeaponBackpack.Instance.GetEquippedBullets().Select(b => b.Address).ToArray();

            NowSD.Anchor = new[]
            {
                Whole.anchorPoints[0].isUnlocked,
                Whole.anchorPoints[1].isUnlocked,
                Whole.anchorPoints[2].isUnlocked,
                Whole.anchorPoints[3].isUnlocked,
                Whole.anchorPoints[4].isUnlocked,
                Whole.anchorPoints[5].isUnlocked
            };
        }
        PlayerPrefs.SetString(num.ToString(), JsonUtility.ToJson(NowSD)); PlayerPrefs.Save();
        Debug.Log("Saving: " + JsonUtility.ToJson(NowSD));
    }

    public static void NewSave()
    {
        // 创建一个新的 SD 实例
        int newSDNum = GetNextSaveName(); // 获取新的存档名称
        SD newSD = new SD
        {
            SDNum = newSDNum, // 将存档编号设置为新名称
            Anchor = new bool[] {false, false, false, false, false, false}, // 初始化锚点状态
            EB = Array.Empty<string>(), // 初始化装备子弹列表
            BB = Array.Empty<string>() // 初始化背包子弹列表
        };
        PlayerPrefs.SetString(newSDNum.ToString(), JsonUtility.ToJson(newSD)); //存起来
        PlayerPrefs.SetInt(SaveCount,newSDNum); PlayerPrefs.Save(); //数目加一
        Load(newSDNum);
    }

    private static int GetNextSaveName()
    {
        int maxNum = PlayerPrefs.GetInt(SaveCount, 0);
        return maxNum + 1; // 返回当前存档计数作为新的存档名称
    }
}
