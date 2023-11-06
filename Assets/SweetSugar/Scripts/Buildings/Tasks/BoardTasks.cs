using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BoardTasks : MonoBehaviour
{
    public Action UpdateActiveTasks;
    
    [Header("Active Stats")]
    [SerializeField] private LocationTasks activeLocation;
    [SerializeField] private LevelDesk activeLevelDesk;
    
    [Header("Preferences Levels")]
    [SerializeField] private List<LocationTasks> locationTasks;
    [SerializeField] private List<LevelDesk> levelDesks;
    [SerializeField] private List<ItemTask> activeTasks;
    
    private int numberActiveLocation;
    private int completedTask;
    
    private BuildingStruct[] buidingStructs;
    private TaskStruct[] activeLocationTaskStructs;

    public List<ItemTask> ActiveTasks => activeTasks;
    public int CompletedTask => completedTask;
    public int CountActiveLocationTasks => activeLocation.Tasks.Count;
    public int NumberActiveLocation => numberActiveLocation;

    [SerializeField] private string savePath;
    [SerializeField] private string saveFileName = "BoardTasksSaves.json";

    public void SaveToFile()
    {
        CreateBuidingsStruct();
        CreateTaskStruct();
        
        BoardTasksStruct boardTaskStruct = new BoardTasksStruct
        {
            Buidings = buidingStructs,
            ActiveLocationTaskStructs = activeLocationTaskStructs
        };
        
        string json = JsonUtility.ToJson(boardTaskStruct, true);
        
        try
        {
            File.WriteAllText(savePath,json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error write to json");
        }
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
            BoardTasksStruct boardTaskStruct = JsonUtility.FromJson<BoardTasksStruct>(json);
            buidingStructs = boardTaskStruct.Buidings;
            activeLocationTaskStructs = boardTaskStruct.ActiveLocationTaskStructs;
            
            if (buidingStructs.Length > 0)
            {
                int i = 0;
                foreach (var building in buidingStructs)
                {
                    activeLevelDesk.buidingInMap[i].CountCreatedObjects = building.CountCreatedObjects;
                    activeLevelDesk.buidingInMap[i].CreateObject(activeLevelDesk.buidingInMap[i].CountCreatedObjects);
                    i++;
                }
            }
            
            if (activeLocationTaskStructs.Length > 0)
            {
                int i = 0;
                foreach (var taskStruct in activeLocationTaskStructs)
                {
                    activeLocation.Tasks[i].CompletedCountTasks = taskStruct.CompletedCountTasks;
                    activeLocation.Tasks[i].IsCompletedTasks = taskStruct.IsCompletedTask;
                    i++;
                }
            }

            Debug.Log("Load from json");
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

        if (!PlayerPrefs.HasKey("Location"))
            PlayerPrefs.SetInt("Location", 0);

        numberActiveLocation = PlayerPrefs.GetInt("Location");
        activeLevelDesk = levelDesks[numberActiveLocation];
        activeLevelDesk.gameObject.SetActive(true);
        activeLocation = locationTasks[numberActiveLocation];
        activeLocationTaskStructs = new TaskStruct[activeLocation.Tasks.Count];
        
        LoadFromFile();
    }

    void Start()
    {
        UpdateBoardTasks();
    }

    public void UpdateBoardTasks()
    {
        if (activeTasks.Count > 0)
            activeTasks.Clear();

        int i = 0;
        completedTask = 0;

        foreach (var task in activeLocation.Tasks)
        {
            if (activeLocationTaskStructs[i].IsCompletedTask)
            {
                i++;
                completedTask++;
                continue;
            }

            if (activeTasks.Count < 2)
                AddTask(task);
            else
                break;
            i++;
        }

        if (activeTasks.Count <= 0)
        {
            UpdateLocation();
        }

        UpdateActiveTasks?.Invoke();
    }

    private void AddTask(ItemTask itemTask)
    {
        activeTasks.Add(itemTask);
    }

    private void UpdateLocation()
    {
        PlayerPrefs.SetInt("Location", numberActiveLocation + 1);
        numberActiveLocation = PlayerPrefs.GetInt("Location");
        
        activeLevelDesk = levelDesks[numberActiveLocation];
        for (int i = 0; i < levelDesks.Count; i++)
        {
            levelDesks[i].gameObject.SetActive(false);
        }
        activeLevelDesk.gameObject.SetActive(true);
        
        activeLocation = locationTasks[numberActiveLocation];
        activeLocationTaskStructs = new TaskStruct[activeLocation.Tasks.Count];

        UpdateBoardTasks();
        SaveToFile();
        Debug.Log("Update Location");
    }
    
    public void UpgradeTask(string name, int completedCountTasks)
    {
        foreach (var task in activeTasks)
        {
            if (task.Name == name)
            {
                task.CompletedCountTasks = completedCountTasks;
                if (task.CompletedCountTasks >= task.CountTasks)
                {
                    task.IsCompletedTasks = true;
                }
            }
        }
        
        SaveToFile();
        UpdateBoardTasks();
    }

    private void CreateTaskStruct()
    {
        activeLocationTaskStructs = new TaskStruct[activeLocation.Tasks.Count];
        
        int i = 0;
        foreach (var task in activeLocation.Tasks)
        {
            TaskStruct taskStruct = new TaskStruct();
            taskStruct.Name = task.Name;
            taskStruct.CompletedCountTasks = task.CompletedCountTasks;
            taskStruct.IsCompletedTask = task.IsCompletedTasks;

            activeLocationTaskStructs[i] = taskStruct;
            i++;
        }
    }

    private void CreateBuidingsStruct()
    {
        buidingStructs = new BuildingStruct[activeLevelDesk.buidingInMap.Count];
        
        int i = 0;
        foreach (var building in activeLevelDesk.buidingInMap)
        {
            BuildingStruct buildingStruct = new BuildingStruct();
            buildingStruct.Name = building.name;
            buildingStruct.CountCreatedObjects = building.CountCreatedObjects;

            buidingStructs[i] = buildingStruct;
            i++;
        }
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
