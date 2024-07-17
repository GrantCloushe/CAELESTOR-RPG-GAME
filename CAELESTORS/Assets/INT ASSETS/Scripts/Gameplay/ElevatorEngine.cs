using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEngine : MonoBehaviour
{
    StairsTrigger elevator;

    [Header("UsageCondition")]
    public bool useItemCondition;
    [SerializeField] private ItemUseTrigger useItem;

    void Start()
    {
        elevator = GetComponent<StairsTrigger>();
    }


    void Update()
    {
        if (useItemCondition)
        {
            elevator.locked = !useItem.isActivated;
        }
    }
}
