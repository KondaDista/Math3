using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTasks : MonoBehaviour
{
    [SerializeField] private List<ItemTask> startTasks = new List<ItemTask>();
    [SerializeField] private List<ItemTask> activeTasks = new List<ItemTask>();
    [SerializeField] private List<Building> buidingInMap = new List<Building>();

    public List<ItemTask> ActiveTasks => activeTasks;
    public List<Building> BuidingInMap => buidingInMap;

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

}
