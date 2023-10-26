using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private List<Transform> CreateObjects;
    public int CountCreatedObjects;

    private void Start()
    {
        CreateObject(CountCreatedObjects);
    }

    public void CreateObject(int numberObject)
    {
        CountCreatedObjects = 0;
        for (int i = 0; i < numberObject; i++)
        {
            CreateObjects[i].gameObject.SetActive(true);
            CountCreatedObjects++;
        }
    }
}
