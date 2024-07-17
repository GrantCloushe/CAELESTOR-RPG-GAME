using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCamera : MonoBehaviour
{
    Transform chaseObject;
    CameraControl cam;
    PhysicObjectController player;
    [SerializeField] private bool enable;
    [SerializeField, Range(0f, 360f)] private float rotationAngle;
    [SerializeField] private float rotationDistance;


    void Start()
    {
        chaseObject = Camera.main.GetComponent<CameraControl>().chaseObject;
        cam = Camera.main.GetComponent<CameraControl>();
        player = GetComponentInParent<PhysicObjectController>();
    }

   
    void Update()
    {
        if (chaseObject != null)
        {
            Vector3 rotationPos = chaseObject.position - transform.forward * rotationDistance;
            transform.position = rotationPos;
            transform.rotation = Quaternion.Euler(0, rotationAngle + chaseObject.eulerAngles.y, 0);
        }
        else
        {
            chaseObject = Camera.main.GetComponent<CameraControl>().chaseObject;
        }

        if (rotationAngle > 360)
        {
            rotationAngle = 0;
        }
        if (rotationAngle < 0)
        {
            rotationAngle = 359;
        }

        if (enable)
        {
            cam.SetViewport(transform, 0, 0.5f, 0);
        }
        else
        {
            if (player.IsPlayable())
            {
                if(cam.Viewport() == transform)
                {
                    cam.SetViewport(player.transform, -8, 0.5f, 2);
                }
            }
        }

    }

    public void VictoryIdle()
    {
        if(enable == false)
        {
            rotationAngle = 360;
            rotationDistance = 8;
            enable = true;
        }

        if(rotationAngle > 180)
        {
            rotationAngle -= 4;
        }
        else
        {
            rotationAngle = 180;
        }
    }
}
