using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    //private void Start()
    //{
    //    LoadAchievements(); // 游戏启动时加载成就数据
    //}

    public void UnlockAchievement(string achievementName)
    {
        // 查找对应名称的成就
        Achievement achievement = Whole.achievements.Find(a => a.name == achievementName);

        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.isUnlocked = true; // 解锁成就
            Debug.Log($"Achievement Unlocked: {achievement.name}");
            SaveAchievements(); // 解锁后保存成就数据
        }
    }

    //public void LoadAchievements()
    //{
    //    // 从PlayerPrefs中加载成就数据
    //    string json = PlayerPrefs.GetString("Achievements", "[]");
    //    List<Achievement> loadedAchievements = JsonUtility.FromJson<AchievementList>(json).achievements;

    //    // 更新Whole.achievements列表
    //    Whole.achievements = loadedAchievements;
    //}

    public void SaveAchievements()
    {
        // 将成就数据转为JSON字符串并保存到PlayerPrefs
        string json = JsonUtility.ToJson(new AchievementList { achievements = Whole.achievements });
        PlayerPrefs.SetString("Achievements", json);
        PlayerPrefs.Save(); // 确保数据被写入
    }
}

// 用于处理Achievement的序列化
[System.Serializable]
public class AchievementList
{
    public List<Achievement> achievements;
}
