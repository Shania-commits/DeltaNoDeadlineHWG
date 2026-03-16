using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public TextMeshProUGUI taskText;

    private int currentTask = 0;

    void Start()
    {
        UpdateTasks();
    }

    public void CompleteTask()
    {
        currentTask++;
        UpdateTasks();
    }

    void UpdateTasks()
    {
        if (currentTask == 0)
        {
            taskText.text =
            "Tasks\n" +
            "[ ] Get Crowbar\n" +
            "[ ] Use Crowbar on Window\n" +
            "[ ] Escape!";
        }

        if (currentTask == 1)
        {
            taskText.text =
            "Tasks\n" +
            "[✓] Get Crowbar\n" +
            "[ ] Use Crowbar on Window\n" +
            "[ ] Escape!";
        }

        if (currentTask == 2)
        {
            taskText.text =
            "Tasks\n" +
            "[✓] Get Crowbar\n" +
            "[✓] Use Crowbar on Window\n" +
            "[ ] Escape!";
        }

        if (currentTask >= 3)
        {
            taskText.text =
            "Tasks\n" +
            "[✓] Get Crowbar\n" +
            "[✓] Use Crowbar on Window\n" +
            "[✓] Escape!";
        }
    }
}