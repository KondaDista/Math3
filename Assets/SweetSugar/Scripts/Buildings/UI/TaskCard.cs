using System;
using System.Collections;
using System.Collections.Generic;
using SweetSugar.Scripts;
using SweetSugar.Scripts.Core;
using SweetSugar.Scripts.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskCard : MonoBehaviour
{
    [HideInInspector] public BoardTasksWindow boardTasksWindow;
    
    public string Name;
    public TMP_Text Title;
    public Image Icon;
    public int Price;
    public TMP_Text PriceText;
    public int CountTasks;
    public int CompletedCountTasks;

    public Button ButtonCreateObject;
    public Building BuildingInMap;
    
    [SerializeField] private Transform SuccsesCreateObject;
    [SerializeField] private GameObject CountTasksSlider;
    [SerializeField] private Image SliderImage;

    private void Start()
    {
        if (CompletedCountTasks >= CountTasks)
        {
            CompleteTask();
            return;
        }

        ButtonCreateObject.onClick.AddListener(ExecutionTask);
        if (CountTasks > 1)
        {
            CountTasksSlider.SetActive(true);
            UpdateSliderValue();
        }
    }

    public void ExecutionTask()
    {
        if (InitScript.Instance.GetCraftedItems() >= Price)
        {
            InitScript.Instance.SpendCraftItems(Price);
            CompletedCountTasks++;
            BuildingInMap.CreateObject(CompletedCountTasks);

            if (CountTasks > 1)
                UpdateSliderValue();

            if (CompletedCountTasks >= CountTasks)
            {
                CompleteTask();
                return;
            }

            boardTasksWindow.UpgradeTask(this);
        }
        else
        {
            SoundBase.Instance.PlayOneShot(SoundBase.Instance.click);
            MenuReference.THIS.BoardTasks.gameObject.SetActive(false);
            MenuReference.THIS.NotCraftedItems.gameObject.SetActive(true);
        }
    }
    
    private void UpdateSliderValue()
    {
        SliderImage.fillAmount = (float)CompletedCountTasks/CountTasks;
    }
    
    public void LoadTask()
    {
        BuildingInMap.CreateObject(CompletedCountTasks);
    }

    private void CompleteTask()
    {
        ButtonCreateObject.interactable = false;
        SuccsesCreateObject.gameObject.SetActive(true);
        CountTasksSlider.SetActive(false);
        
        boardTasksWindow.UpgradeTask(this);
    }
}
