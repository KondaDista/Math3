using System;
using System.Collections;
using System.Collections.Generic;
using SweetSugar.Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskCard : MonoBehaviour
{
    public TMP_Text Name;
    public Image Icon;
    public int Price;
    public TMP_Text PriceText;
    public int CountTasks;
    public int CompletedCountTasks;

    public Button ButtonCreateObject;
    public Transform SuccsesCreateObject;
    public Building BuildingInMap;
    
    [SerializeField] private GameObject CountTasksSlider;

    private void Start()
    {
        if (CompletedCountTasks >= CountTasks)
        {
            CompleteTask();
            return;
        }

        ButtonCreateObject.onClick.AddListener(CreateObjectInBuilding);
        if (CountTasks > 1)
        {
            CountTasksSlider.SetActive(true);
        }
    }

    public void CreateObjectInBuilding()
    {
        if (InitScript.Instance.GetCraftedItems() >= Price)
        {
            InitScript.Instance.SpendCraftItems(Price);
            CompletedCountTasks++;
            BuildingInMap.CreateObject(CompletedCountTasks);

            if (CompletedCountTasks >= CountTasks)
                CompleteTask();
        }
    }
    
    public void LoadTask()
    {
        BuildingInMap.CreateObject(CompletedCountTasks);
    }

    private void CompleteTask()
    {
        ButtonCreateObject.interactable = false;
        SuccsesCreateObject.gameObject.SetActive(true);
    }
}
