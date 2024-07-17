using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TittleCameraRotate : MonoBehaviour
{
    [SerializeField] private float RotateSpeed;

    void Update()
    {
        transform.Rotate(0, RotateSpeed, 0);
    }
}
