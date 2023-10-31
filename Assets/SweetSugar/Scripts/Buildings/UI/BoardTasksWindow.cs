using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class BoardTasksWindow : MonoBehaviour
{
    [SerializeField] private BoardTasks boardTasks;

    [SerializeField] private RectTransform boardPanel;
    [SerializeField] private List<TaskCard> currentTaskCard;
    [SerializeField] private TaskCard prefabTaskCard;
    
    private void OnEnable()
    {
        boardTasks.UpdateActiveTasks += Redraw;
    }

    private void OnDisable()
    {
        boardTasks.UpdateActiveTasks -= Redraw;
    }

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
            TaskCard taskCard = Instantiate(prefabTaskCard, boardPanel);
            taskCard.boardTasksWindow = this;
            taskCard.Name = task.Name;
            taskCard.Title.text = task.Title;
            taskCard.Icon.sprite = task.Icon;
            taskCard.Price = task.Price;
            taskCard.PriceText.text = task.Price.ToString();
            taskCard.CountTasks = task.CountTasks;
            taskCard.CompletedCountTasks = task.CompletedCountTasks;
            taskCard.BuildingInMap = task.Building;

            currentTaskCard.Add(taskCard);
            taskCard.LoadTask();
            i++;
        }
    }

    public void UpgradeTask(TaskCard taskCard)
    {
        boardTasks.UpgradeTask(taskCard.Name, taskCard.CompletedCountTasks);
    }

    private void ClearDrawn()
    {
        foreach (var taskCard in currentTaskCard)
        {
            Destroy(taskCard.gameObject);
        }
        currentTaskCard.Clear();
    }
}
