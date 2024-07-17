using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaling : MonoBehaviour
{
    public float ScaleMultiplier;
    public float MaxScale = 5;
    private Vector3 initialScale;
    private Vector3 sizeScale;
    [SerializeField] private bool clampScale;

    // Start is called before the first frame update
    void Awake()
    {
        initialScale = transform.localScale;

        transform.localScale = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        if (clampScale)
        {
            Clamp(distance);
        }
        else
        {
            sizeScale = (initialScale / distance) * ScaleMultiplier;
            if (sizeScale.x < MaxScale)
            {
                transform.localScale = sizeScale;
            }
        }
      
    }

    void Clamp(float distance)
    {
        if (distance > 2)
        {
            sizeScale = (initialScale / distance) * ScaleMultiplier;

            if (sizeScale.x < initialScale.x)
            {
                transform.localScale = sizeScale;
            }
            else
            {
                transform.localScale = initialScale;
            }
        }
        else
        {
            transform.localScale = initialScale;
        }
    }
}
