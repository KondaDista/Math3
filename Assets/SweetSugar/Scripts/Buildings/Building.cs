using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private List<Transform> CreateObjects;

    public void CreateObject(int numberObject)
    {
        CreateObjects[numberObject].gameObject.SetActive(true);
    }
}
