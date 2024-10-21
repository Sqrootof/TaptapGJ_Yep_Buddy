using UnityEngine;

[System.Serializable]
public class Task
{
    public string taskName; // 任务名称
    public string[] description; // 任务描述
    public Vector3[] trackingCoordinates; // 追踪的坐标
    public int Process;
    public int EndProcess;
}

public interface ITaskManager
{
    //添加任务
    public void AddTask(Task task);


    //添加任务
    public void AddTaskName(string taskName, string[] description, Vector3[] vector3s, int EndProgress);


    //放弃任务
    public void RemoveTask(string taskName);


    //任务进程增加
    public void ProgressIncrease(string taskName);


    //任务完成
    public void OnTaskComplete(Task completedTask);


    //切换追踪任务
    public void ChangeTrackingTask(string taskName);
}
