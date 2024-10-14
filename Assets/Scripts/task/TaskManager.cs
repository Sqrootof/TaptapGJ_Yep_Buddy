using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    //public List<Task> questList = new List<Task>(); // 任务列表
    private Task trackedTask = null; // 当前被追踪的任务

    // 添加任务
    public void AddQuest(Task task)
    {
        Whole.questList.Add(task);
    }

    // 切换任务追踪状态
    public void ToggleTracking(Task task)
    {
        if (task.isTracking)
        {
            // 取消追踪
            task.isTracking = false;
            trackedTask = null;
            Debug.Log("Tracking canceled for: " + task.taskName);
        }
        else
        {
            // 如果已经有一个任务在追踪，先取消它的追踪
            if (trackedTask != null)
            {
                trackedTask.isTracking = false;
            }

            // 追踪当前任务
            task.isTracking = true;
            trackedTask = task;
            Debug.Log("Tracking task: " + task.taskName);
        }
    }
}