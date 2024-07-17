using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    Animator anim;
    PhysicObjectController obj;

    void Start()
    {
        anim = GetComponent<Animator>();
        obj = GetComponentInParent<PhysicObjectController>();
    }


    void Update()
    {
        if (!obj.isCarrying)
        {
            anim.SetInteger("Action", obj.currentAction);
        }
        else
        {
            anim.SetInteger("Action", 0);
        }
    }
}
