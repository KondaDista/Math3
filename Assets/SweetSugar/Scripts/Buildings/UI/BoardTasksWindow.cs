using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BoardTasksWindow : MonoBehaviour
{
    [SerializeField] private BoardTasks boardTasks;

    [SerializeField] private RectTransform boardPanel;
    [SerializeField] private List<TaskCard> currentTaskCard;
    [SerializeField] private TaskCard prefabTaskCard;
    void Start()
    {
        prefabTaskCard = Resources.Load<TaskCard>("Prefabs/Tasks/TaskCard");
    }

    public void Redraw()
    {
        ClearDrawn();
        int i = 0;
        foreach (var task in boardTasks.ActiveTasks)
        {
            var taskCard = Instantiate(prefabTaskCard, boardPanel);
            taskCard.Name.text = task.Name;
            taskCard.Icon.sprite = task.Icon;
            taskCard.Price = task.Price;
            taskCard.PriceText.text = task.Price.ToString();
            taskCard.CountTasks = task.CountTasks;

            taskCard.BuildingInMap = boardTasks.BuidingInMap[i];
            currentTaskCard.Add(taskCard);
            i++;
        }
    }

    private void ClearDrawn()
    {
        foreach (var task in currentTaskCard)
        {
            Destroy(task.gameObject);
        }
        currentTaskCard.Clear();
    }
}
