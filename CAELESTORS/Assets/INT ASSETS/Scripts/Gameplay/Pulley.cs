using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulley : MonoBehaviour
{
    Transform pulley;
    [SerializeField] private int currentStation;
    [SerializeField] private Transform[] stations;
    [SerializeField] private int transportDirection;
    [SerializeField] private float height;
    public bool active;
    private bool changeLocation;

    void Start()
    {
        pulley = GetComponent<Transform>();
        transportDirection = 1;
    }

    void Update()
    {
        if(stations != null)
        {
            Transform curStation = stations[currentStation];
            pulley.position = new Vector3(curStation.position.x, curStation.position.y + (3+height), curStation.position.z);
            WorkingSystem();
        }
    }

    void WorkingSystem()
    {
        if (active)
        {
            if (!changeLocation)
            {
                height += 0.05f;

                if(height > 5)
                {
                    changeLocation = true;
                    currentStation = currentStation + transportDirection;
                }
            }

            if (changeLocation)
            {
                height -= 0.05f;

                if(height < 0)
                {
                    changeLocation = false;
                    height = 0;
                    active = false;
                }
            }
        }

        if(currentStation == stations.Length - 1)
        {
            transportDirection = -1;
        }

        if(currentStation == 0)
        {
            transportDirection = 1;
        }
    }
}
