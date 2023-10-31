using UnityEngine;

[System.Serializable]
public class ItemTask 
{
   public string Name = "Task_0";
   public string Title = "Title";
   public Sprite Icon;
   public int Price = 1;
   public int CountTasks = 1;
   public int CompletedCountTasks = 0;
   public bool IsCompletedTasks = false;
   
   public Building Building;
}
