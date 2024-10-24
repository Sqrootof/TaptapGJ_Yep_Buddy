using UnityEngine;

public class AddTaskTest : MonoBehaviour
{
    public Task task;
    private TaskManager taskManager;
    public void add()
    {
        taskManager = new TaskManager();
        taskManager.AddTask(task);
        Debug.Log("1");
    }
    void Start()
    {

    }

    void Update()
    {
        
    }
}
