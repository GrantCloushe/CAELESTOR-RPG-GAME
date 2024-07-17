using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    Animator anim;
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private int gearRation;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        anim.speed = rotatingSpeed;
    }
}
