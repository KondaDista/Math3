using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "ItemTasks/Task")]
public class ItemTask : ScriptableObject
{
   public string Name = "Task";
   public Sprite Icon;
   public int Price = 1;
   public int CountTasks = 1;
}
