using UnityEngine;

public enum TaskStatus
{
    NotStarted,
    InProgress,
    Completed
}

[System.Serializable]
public class Task
{
    public string taskName; // 任务名称
    public string description; // 任务描述
    public Vector3 trackingCoordinates; // 追踪的坐标

    public bool isAccepted = false; // 是否已接取
    public bool isCompleted = false; // 是否已完成
    public bool isTracking = false; // 是否正在被追踪

    public TaskStatus status = TaskStatus.NotStarted;

    public void AcceptTask()
    {
        isAccepted = true;
        status = TaskStatus.InProgress;
        Debug.Log("Task accepted: " + taskName);
    }
    public void CompleteQuest()
    {
        isCompleted = true;
        status = TaskStatus.Completed;
        Debug.Log("Task completed: " + taskName);
    }

    // 检查任务是否显示在任务栏
    public bool ShouldDisplayInUI()
    {
        return isAccepted && !isCompleted;
    }

    // 判断是否能完成任务 (可以根据你具体的判定方法更新)
    public bool CanComplete()
    {
        return isCompleted;
    }
}
