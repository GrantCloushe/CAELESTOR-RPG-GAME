using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Build : MonoBehaviour
{
    TaskPapperUI taskPapper;
    [SerializeField] private Sprite taskImage;
    [SerializeField] private GameObject buildingTarget;
    [SerializeField] private bool isNotOnThisStage;
    void Start()
    {
        taskPapper = FindObjectOfType<TaskPapperUI>();
        taskPapper.TaskImage(taskImage);
    }

    void Update()
    {
        
    }

    public bool TaskStatus()
    {
        if (buildingTarget.activeInHierarchy && !isNotOnThisStage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
