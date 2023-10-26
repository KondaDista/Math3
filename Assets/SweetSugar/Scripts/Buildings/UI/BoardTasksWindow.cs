using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

public class BoardTasksWindow : MonoBehaviour
{
    [SerializeField] private BoardTasks boardTasks;

    [SerializeField] private RectTransform boardPanel;
    [SerializeField] private List<TaskCard> currentTaskCard;
    [SerializeField] private TaskCardStruct[] TaskCardStructs;
    [SerializeField] private TaskCard prefabTaskCard;
    
    [SerializeField] private string savePath;
    [SerializeField] private string saveFileName = "TasksCardSaves.json";
    
    public void SaveToFile()
    {
        CreateTaskStruct();
        BoardTaskWindowStruct boardTaskWindowStruct = new BoardTaskWindowStruct
        {
            currentTaskCardStructs = TaskCardStructs
        };
        
        string json = JsonUtility.ToJson(boardTaskWindowStruct, true);
        
        try
        {
            File.WriteAllText(savePath,json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error write to json");
        }
        
        boardTasks.SaveToFile();
    }
    
    public void LoadFromFile()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("File not found");
            return;
        }

        string json = File.ReadAllText(savePath);

        try
        {
            BoardTaskWindowStruct boardTaskWindowStruct = JsonUtility.FromJson<BoardTaskWindowStruct>(json);
            TaskCardStructs = boardTaskWindowStruct.currentTaskCardStructs;

            if (TaskCardStructs.Length > 0)
            {
                int i = 0;
                foreach (var taskCardStruct in TaskCardStructs)
                {
                    TaskCard taskCard = Instantiate(prefabTaskCard, boardPanel);
                    taskCard.Name.text = taskCardStruct.Name;
                    taskCard.Icon.sprite = taskCardStruct.Icon;
                    taskCard.Price = taskCardStruct.Price;
                    taskCard.PriceText.text = taskCardStruct.Price.ToString();
                    taskCard.CountTasks = taskCardStruct.CountTasks;
                    taskCard.CompletedCountTasks = taskCardStruct.CompletedCountTasks;
                    taskCard.ButtonCreateObject.onClick.AddListener(SaveToFile);
                    taskCard.BuildingInMap = boardTasks.BuidingInMap[i];
                    
                    currentTaskCard.Add(taskCard);
                    taskCard.LoadTask();
                    i++;
                }
            }
            Debug.Log("Load tasks from json");
        }
        catch (Exception e)
        {
            Debug.LogError("Error load from json");
        } 
    }
    
    private void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        savePath = Path.Combine(Application.persistentDataPath, saveFileName);
#else
        savePath = Path.Combine(Application.dataPath, saveFileName);
#endif
        LoadFromFile();
    }
    
    void Start()
    {
        prefabTaskCard = Resources.Load<TaskCard>("Prefabs/Tasks/TaskCard");
    }

    public void Redraw()
    {
        ClearDrawn();
        LoadFromFile();
        if (currentTaskCard.Count <= 0)
        {
            int i = 0;
            foreach (var task in boardTasks.ActiveTasks)
            {
                TaskCard taskCard = Instantiate(prefabTaskCard, boardPanel);
                taskCard.Name.text = task.Name;
                taskCard.Icon.sprite = task.Icon;
                taskCard.Price = task.Price;
                taskCard.PriceText.text = task.Price.ToString();
                taskCard.CountTasks = task.CountTasks;
                taskCard.ButtonCreateObject.onClick.AddListener(SaveToFile);
                taskCard.BuildingInMap = boardTasks.BuidingInMap[i];
                
                currentTaskCard.Add(taskCard);
                i++;
            }
        }

    }
    
    private void CreateTaskStruct()
    {
        TaskCardStructs = new TaskCardStruct[currentTaskCard.Count];
        int i = 0;
        
        foreach (var task in currentTaskCard)
        {
            TaskCardStruct taskStruct = new TaskCardStruct();
            taskStruct.Name = task.Name.text;
            taskStruct.Icon = task.Icon.sprite;
            taskStruct.Price = task.Price;
            taskStruct.CountTasks = task.CountTasks;
            taskStruct.CompletedCountTasks = task.CompletedCountTasks;
            
            TaskCardStructs[i] = taskStruct;
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

    private void OnApplicationQuit()
    {
        SaveToFile();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            SaveToFile();
        }
    }
}
