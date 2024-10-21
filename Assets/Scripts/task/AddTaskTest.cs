using UnityEngine;

public class AddTaskTest : MonoBehaviour
{
    public Task task;
    private TaskManager taskManager;
    public void add()
    {
        taskManager.AddTask(task);
    }
    void Start()
    {
        taskManager = new TaskManager();
        add();
    }

    void Update()
    {
        
    }
}
