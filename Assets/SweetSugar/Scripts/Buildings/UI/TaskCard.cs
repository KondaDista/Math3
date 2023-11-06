using System;
using System.Collections;
using System.Collections.Generic;
using SweetSugar.LeanTween.Framework;
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

    public void FirstOpenTask()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 1f).setDelay(1f)
            .setEase(LeanTweenType.easeOutQuint);
    }

    public void ExecutionTask()
    {
        if (InitScript.Instance.GetCraftedItems() >= Price)
        {
            InitScript.Instance.SpendCraftItems(Price);
            CompletedCountTasks++;

            if (CountTasks > 1)
                UpdateSliderValue();

            if (CompletedCountTasks >= CountTasks)
            {
                CompleteTask();
            }

            UpgradeBuilding();
            boardTasksWindow.UpgradeTask(this);
            boardTasksWindow.transform.GetChild(0).gameObject.SetActive(false);
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
    
    public void UpgradeBuilding()
    {
        BuildingInMap.CreateObject(CompletedCountTasks);
    }

    private void CompleteTask()
    {
        ButtonCreateObject.interactable = false;
        SuccsesCreateObject.gameObject.SetActive(true);
        CountTasksSlider.SetActive(false);
    }

    public void DeletedTask()
    {
        LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), 0.75f).setDelay(0.2f)
            .setEase(LeanTweenType.easeOutQuint).setOnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}
