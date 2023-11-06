using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardTasksWindow : MonoBehaviour
{
    [SerializeField] private BoardTasks boardTasks;

    [SerializeField] private RectTransform boardPanel;
    [SerializeField] private List<TaskCard> currentTaskCard;
    [SerializeField] private TaskCard prefabTaskCard;
    
    [SerializeField] private Image SliderCompletedTask;
    [SerializeField] private TMP_Text ProgressTaskText;
    [SerializeField] private TMP_Text LocationNumberText;

    private bool IsActive = false;
    
    private void OnEnable()
    {
        boardTasks.UpdateActiveTasks += UpdateSliderValue;
    }

    private void OnDisable()
    {
        boardTasks.UpdateActiveTasks -= UpdateSliderValue;
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
            if (currentTaskCard.Count > 0 && currentTaskCard.Count(card => card.Name == task.Name) > 0) 
            {
                i++;
                continue;
            }
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

            taskCard.UpgradeBuilding();
            if (IsActive && taskCard.CompletedCountTasks <= 0 )
                    taskCard.FirstOpenTask();
            
            currentTaskCard.Add(taskCard);
            i++;
        }

        if (!IsActive)
            IsActive = true;
    }
    
    private void UpdateSliderValue()
    {
        SliderCompletedTask.fillAmount = (float)boardTasks.CompletedTask / boardTasks.CountActiveLocationTasks;
        ProgressTaskText.text = $"{boardTasks.CompletedTask}/{boardTasks.CountActiveLocationTasks}";
        LocationNumberText.text = $"Zone {boardTasks.NumberActiveLocation + 1}";
    }

    public void UpgradeTask(TaskCard taskCard)
    {
        boardTasks.UpgradeTask(taskCard.Name, taskCard.CompletedCountTasks);
    }

    private void ClearDrawn()
    {
        foreach (var taskCard in currentTaskCard)
        {
            if (taskCard.CompletedCountTasks == taskCard.CountTasks)
            {
                taskCard.DeletedTask();
                currentTaskCard.Remove(taskCard);
                break;
            }
        }
    }
}
