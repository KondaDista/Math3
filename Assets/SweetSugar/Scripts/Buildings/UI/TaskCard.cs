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
    private int currentCountTasks = 0;
    
    public Button ButtonCreateObject;
    public Transform SuccsesCreateObject;
    public Building BuildingInMap;
    
    [SerializeField] private GameObject CountTasksSlider;

    private void Start()
    {
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
            BuildingInMap.CreateObject(currentCountTasks);
            currentCountTasks++;

            if (currentCountTasks >= CountTasks)
            {
                SuccsesCreateObject.gameObject.SetActive(true);
            }
        }
    }
}
