using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    Animator anim;
    private bool activated;
    [SerializeField] private float activeTime;
    [SerializeField] private float jumpTime;
    [SerializeField] private float maxTime;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    void Update()
    {
        anim.SetBool("Active", activated);

        if (activated)
        {
            activeTime++;
        }

        if(activeTime > maxTime)
        {
            activated = false;
        }
    }

    public void Active()
    {
        activeTime = 0;
        activated = true;
    }

    public bool JumpState()
    {
        if(activeTime < jumpTime)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
