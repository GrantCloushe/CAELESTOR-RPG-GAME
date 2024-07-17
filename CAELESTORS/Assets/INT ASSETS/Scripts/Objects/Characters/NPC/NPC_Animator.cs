using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Animator : MonoBehaviour
{
    private NPC_Physics obj;
    private Animator anim;
    private SpriteDraw sprDraw;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sprDraw = GetComponent<SpriteDraw>();
        obj = GetComponentInParent<NPC_Physics>();
    }

    void Update()
    {
        anim.SetBool("isWalking", obj.isWalking());
        anim.SetFloat("Angle", sprDraw.ang);
        anim.SetInteger("Action", obj.action);

        if (obj.isWalking())
        {
            anim.speed = obj.Speed();
        }
        else
        {
            anim.speed = 1;
        }
    }
}
