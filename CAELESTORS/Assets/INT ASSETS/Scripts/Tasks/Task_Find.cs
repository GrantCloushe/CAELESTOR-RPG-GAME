using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Find : MonoBehaviour
{
    TaskPapperUI taskPapper;
    CharacterInventory inventory;

    [SerializeField] private Sprite taskImage;
    [SerializeField] private int resourceTracker;
    [SerializeField] private int resourceCountTarget;
    [SerializeField] private int resourceCountCurrent;

    [SerializeField] private List<ResourcePoint> resourceSpawnPoints;

    void Start()
    {
        taskPapper = FindObjectOfType<TaskPapperUI>();
        taskPapper.TaskImage(taskImage);
        inventory = FindObjectOfType<CharacterInventory>();

        for(int i = 0; i < resourceSpawnPoints.Count; i++)
        {
            int randomPoint = Random.Range(0, resourceSpawnPoints[i].spawnpoints.Length);
            int randomResource = Random.Range(0, resourceSpawnPoints[i].resourceToSpawn.Length);
            Transform curPoint = resourceSpawnPoints[i].spawnpoints[randomPoint];
            GameObject curRes = resourceSpawnPoints[i].resourceToSpawn[randomResource];
            Spawn(curPoint, curRes);
        }
    }

    void Update()
    {
        resourceCountCurrent = inventory.GetResource(resourceTracker); 
    }

    public bool TaskStatus()
    {
        if (resourceCountCurrent >= resourceCountTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Spawn(Transform rPoint, GameObject rRes)
    {
        Debug.Log(rRes);
        Debug.Log(rPoint);
        Debug.Log("Spawned");
        Instantiate(rRes);
        rRes.transform.localPosition = rPoint.position + new Vector3(0, 5, 0); 
    }

    [System.Serializable]
    public struct ResourcePoint
    {
        public Transform[] spawnpoints;
        public GameObject[] resourceToSpawn;
    }
}
