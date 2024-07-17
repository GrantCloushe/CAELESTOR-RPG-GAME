using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCamera : MonoBehaviour
{
    [SerializeField] private float rotation;
    [SerializeField] private float pitch;
    [SerializeField] private float yaw;

    public Vector3 movementDirection;
    public float movementSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += movementDirection * movementSpeed;
        transform.rotation = Quaternion.Euler(pitch, rotation, yaw);
    }
}
