using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class BoardTasks : MonoBehaviour
{
    [SerializeField] private List<ItemTask> startTasks = new List<ItemTask>();
    [SerializeField] private List<ItemTask> activeTasks = new List<ItemTask>();
    [SerializeField] private List<Building> buidingInMap = new List<Building>();
    [SerializeField] private BuildingStruct[] buidingStructs;

    public List<ItemTask> ActiveTasks => activeTasks;
    public List<Building> BuidingInMap => buidingInMap;

    [SerializeField] private string savePath;
    [SerializeField] private string saveFileName = "BoardTasksSaves.json";

    public void SaveToFile()
    {
        CreateBuidingsStruct();
        BoardTaskStruct boardTaskStruct = new BoardTaskStruct
        {
            buidings = buidingStructs
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
            BoardTaskStruct boardTaskStruct = JsonUtility.FromJson<BoardTaskStruct>(json);
            buidingStructs = boardTaskStruct.buidings;
            
            if (buidingStructs.Length > 0)
            {
                int i = 0;
                foreach (var building in buidingStructs)
                {
                    buidingInMap[i].CountCreatedObjects = building.CountCreatedObjects;
                    i++;
                }
            }
            
            Debug.Log("Load building from json");
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
        foreach (var task in startTasks)
        {
            AddTask(task);
        }
    }

    private void AddTask(ItemTask itemTask)
    {
        activeTasks.Add(itemTask);
    }
    
    private void CreateBuidingsStruct()
    {
        buidingStructs = new BuildingStruct[buidingInMap.Count];
        
        int i = 0;
        foreach (var building in buidingInMap)
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
