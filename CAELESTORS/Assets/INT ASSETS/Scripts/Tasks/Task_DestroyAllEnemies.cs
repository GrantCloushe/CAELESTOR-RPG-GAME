using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_DestroyAllEnemies : MonoBehaviour
{
    TaskPapperUI taskPapper;
    [SerializeField] private Sprite taskImage;
    [SerializeField] private GameObject enemies;
    [SerializeField] private int enemyCount;

    private void Awake()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemies != null)
        {
            enemies.SetActive(true);
        }
    }

    void Start()
    {
        taskPapper = FindObjectOfType<TaskPapperUI>();
        taskPapper.TaskImage(taskImage);
    }

    
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public bool TaskStatus()
    {
        if (enemyCount == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
