using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] pathPoints;
    [SerializeField] private float speed;
    private int index;
    
    void Update()
    {
        Vector3 pointPos = pathPoints[index].transform.position;
        Vector3 moveToPoint = Vector3.MoveTowards(transform.position, pointPos, speed * Time.deltaTime);
        transform.position = moveToPoint;

        float distance = Vector3.Distance(transform.position, pointPos);

        if (distance < 0.015f)
        {
            if (index < pathPoints.Length - 1)
            {
                index++;

            }
            else
            {
                index = 0;
            }
        }
    }
}
