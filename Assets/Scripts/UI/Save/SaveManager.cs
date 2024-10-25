using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SD
{
    public int SDname;
    public int SDNum;
    public List<AnchorPoint> AP;
    public List<Bullet> EB;
    public List<Bullet> BB;
}

public class SaveManager : MonoBehaviour
{
    public static SD NowSD;
    public static int Num;
    public const string LastUsed = "LastUsedSave";
    public static string NowName;
    public static bool IsLoggedIn = false;
    private string lastUsedSave;

    private void Start()
    {
        lastUsedSave = PlayerPrefs.GetString(LastUsed, "");
        if (!string.IsNullOrEmpty(lastUsedSave) && PlayerPrefs.HasKey(lastUsedSave))
        {
            IsLoggedIn = true;
        }

        if (IsLoggedIn)
        {
            Load(lastUsedSave);
        }
    }

    public static void Load(string name)
    {
        string SDJson = PlayerPrefs.GetString(name, "");
        if (!string.IsNullOrEmpty(SDJson))
        {
            NowSD = JsonUtility.FromJson<SD>(SDJson);
            NowName = NowSD.SDname.ToString();
            Num = NowSD.SDNum;
            IsLoggedIn = true;
        }
    }

    public static void Save()
    {
        NowSD.BB = WeaponBackpack.Instance.GetBulletInBackpack();
        NowSD.EB = WeaponBackpack.Instance.GetEquippedBullets();
        string SDJson = JsonUtility.ToJson(NowSD);
        PlayerPrefs.SetString(NowName, SDJson);
        PlayerPrefs.SetString(LastUsed, NowName); // 更新最后使用的存档
        PlayerPrefs.Save();
    }

    public static void NewSave()
    {
        // 创建一个新的 SD 实例
        int newSDName = GetNextSaveName(); // 获取新的存档名称

        SD newSD = new SD
        {
            SDname = newSDName,
            SDNum = 0,
            AP = new List<AnchorPoint>(),
            EB = new List<Bullet>(),
            BB = new List<Bullet>()
        };

        // 保存新的 SD 实例
        NowSD = newSD;
        NowName = newSD.SDname.ToString();
        Save(); // 保存新的 SD 实例
    }

    private static int GetNextSaveName()
    {
        int maxName = 0;

        // 这里我们假设存档名称是数字字符串，所以需要手动跟踪
        for (int i = 0; i < PlayerPrefs.GetInt("SaveCount", 0); i++)
        {
            if (PlayerPrefs.HasKey(i.ToString()))
            {
                maxName = Math.Max(maxName, i);
            }
        }

        return maxName + 1;
    }
}
