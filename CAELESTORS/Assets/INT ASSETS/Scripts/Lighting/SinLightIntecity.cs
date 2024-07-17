using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinLightIntecity : MonoBehaviour
{
    Light lighting;
    [SerializeField] private float amp;
    [SerializeField] private float constant;
    [SerializeField] private float time;

    void Start()
    {
        lighting = GetComponent<Light>();
    }

    
    void Update()
    {
        lighting.intensity = constant + Mathf.Sin(Time.time / time) * amp;
    }
}
