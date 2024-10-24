using System.Collections.Generic;
using UnityEngine;

public class TaskManager : ITaskManager
{
    //添加任务
    public void AddTask(Task task)
    {
        if (task != null)
        {
            Whole.questList.Add(task);
            if (Whole.theQuest == null)
            {
                Whole.theQuest = task;
            }
        }
    }

    //添加任务
    public void AddTaskName(string taskName, string[] description, Vector3[] vector3s, int EndProgress)
    {
        Task task = new Task();
        task.trackingCoordinates = vector3s;
        task.description = description;
        task.taskName = taskName;
        task.Process = 0;
        task.EndProcess = EndProgress;
        Whole.questList.Add(task);
        if (Whole.theQuest == null)
        {
            Whole.theQuest = task;
        }
    }

    //放弃任务
    public void RemoveTask(string taskName)
    {
        Task taskToRemove = Whole.questList.Find(task => task.taskName == taskName);

        if (taskToRemove != null)
        {
            Whole.questList.Remove(taskToRemove);
            if (Whole.theQuest == taskToRemove)
            {
                Whole.theQuest = null;
            }
        }
        else
        {
            Debug.LogWarning($"Task '{taskName}' not found in quest list.");
        }
    }

    //任务进程增加
    public void ProgressIncrease(string taskName)
    {
        Task task = Whole.questList.Find(t => t.taskName == taskName);

        if (task == null)
        {
            Debug.LogWarning($"Task '{taskName}' not found in quest list.");
            return;
        }

        task.Process++;

        if (task.Process >= task.EndProcess)
        {
            task.Process = task.EndProcess;
            OnTaskComplete(task);
        }
    }

    //任务完成
    public void OnTaskComplete(Task completedTask)
    {
        Debug.Log("任务完成获得奖励");
        //Whole.questList.Remove(completedTask);
        if (Whole.theQuest == completedTask)
        {
            Whole.theQuest = null;
        }
        Debug.Log($"Completed task: {completedTask.taskName}");
    }

    //切换追踪任务
    public void ChangeTrackingTask(string taskName)
    {
        Task task = Whole.questList.Find(t => t.taskName == taskName);
        if (task != null)
        {
            Whole.theQuest = task;
        }
    }
}