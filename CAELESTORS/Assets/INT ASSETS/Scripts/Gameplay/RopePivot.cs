using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePivot : MonoBehaviour
{
    [SerializeField] private float angleX;
    [SerializeField] private float angleY;
    [SerializeField] private float gravity;
    [SerializeField] private float initialAngle;
    [SerializeField] private float lenght;

    void Start()
    {
        
    }


    void Update()
    {
        transform.rotation = Quaternion.Euler(angleX, angleY, 0);

        angleX = initialAngle * Mathf.Cos(Mathf.Sqrt(gravity / lenght) * Time.time);

        if(initialAngle > 1.25f)
        {
            initialAngle -= 0.05f;
        }
    }

    public void GetAmplitude(float amp)
    {
        initialAngle = amp;
    }
}
