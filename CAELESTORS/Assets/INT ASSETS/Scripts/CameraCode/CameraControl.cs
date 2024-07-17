using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Mode7Projection projection;

    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;
    [SerializeField] private float Rotation;
    [SerializeField] private float Pitch;
    [SerializeField] private float Yaw;
    [SerializeField, Min(0)] private float height;
    [SerializeField] private int clampHeight;
    [SerializeField] private Transform selected_viewport;
    [SerializeField] private int follow_offset;
    [SerializeField, Range(0f,12f)] private float easing_speed;
    [HideInInspector]public float easing_time;
    public Transform chaseObject;

    private float r;
    Transform viewport;

    void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;

    }

    void Update()
    {
        transform.position = new Vector3(Mathf.Round(x * 100.0f) * 0.01f, Mathf.Round(y * 100.0f) * 0.01f + height, Mathf.Round(z * 100.0f) * 0.01f);
        transform.rotation = Quaternion.Euler(Mathf.RoundToInt(Pitch), Mathf.RoundToInt(Rotation), Mathf.RoundToInt(Yaw));

        if (viewport != selected_viewport)
        {
            easing_time = 0;
            viewport = selected_viewport;
        }

        if (selected_viewport != null)
        {
            if(easing_speed == 0 || easing_time >= 1)
            {
                ViewportSelected();
            }
            if(easing_speed > 0 && easing_time < 1)
            {
                Easing();
            }
        }

        projection.HeightScale(Pitch);
    }

    void ViewportSelected()
    {
        Vector3 position = selected_viewport.position - selected_viewport.forward * -follow_offset;
        float angle = selected_viewport.eulerAngles.y;

        x = position.x;
        y = position.y + height;
        z = position.z;

        Rotation = angle;
        y = Mathf.Clamp(y, 0, clampHeight);
    }

    void Easing()
    {
        easing_time += Time.deltaTime; 
        Vector3 position = selected_viewport.position - selected_viewport.forward * -follow_offset;
        Vector3 easing = Vector3.Lerp(transform.position, position, easing_time/easing_speed);
        float angle = Mathf.SmoothDampAngle(Rotation, selected_viewport.eulerAngles.y, ref r, easing_speed / 10f);

        x = easing.x;
        z = easing.z;
        Rotation = angle;
    }

    public void SetViewport(Transform viewport, int distance, float _height, float easing)
    {
        selected_viewport = viewport;
        follow_offset = distance;
        height = _height;
        easing_speed = easing;
    }

    public void ChangeDistance(int dist)
    {
        follow_offset = dist;
    }

    public void SetPosition(float setX, float setY, float setZ)
    {
        x = setX;
        y = setY;
        z = setZ;
    }
    
    public void SetAngle(float setRotation, float setPitch, float setYaw)
    {
        Rotation = setRotation;
        Pitch = setPitch;
        Yaw = setYaw;
    }

    public Transform Viewport()
    {
        return viewport;
    }
}
