using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPYPAchievement : MonoBehaviour
{
    AchievementManager achievementManager;
    private void Start()
    {
        achievementManager = GetComponent<AchievementManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            // 找到对应成就的索引
            int achievementIndex = Whole.achievements.FindIndex(a => a.name == "第二个成就");

            // 检查成就是否存在且未解锁
            if (achievementIndex != -1 && Whole.achievements[achievementIndex].isUnlocked == false)
            {
                achievementManager.UnlockAchievement("第二个成就");
            }
        }
    }
}
