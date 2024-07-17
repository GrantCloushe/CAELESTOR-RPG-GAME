using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEffect : MonoBehaviour
{
    SpriteDraw spr;
    Animator anim;
    PhysicObjectController obj;

    void Start()
    {
        spr = GetComponent<SpriteDraw>();
        anim = GetComponent<Animator>();
        obj = GetComponentInParent<PhysicObjectController>();
    }

    void Update()
    {
        anim.SetInteger("Action", obj.currentAction);

        if(obj.currentAction != 9)
        {
            spr.spriteVisible = false;
        }
        else
        {
            spr.spriteVisible = true;
        }
    }
}
