using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propellor : MonoBehaviour
{
    Animator anim;
    [SerializeField] private bool state;
    public Vector3 propellorForce;
    [SerializeField] private float propellorCharging;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        anim.SetBool("State", state);
    }
}
