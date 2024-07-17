using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimationAngle : MonoBehaviour
{
    SpriteDraw spr;
    Animator anim;

    void Start()
    {
        spr = GetComponent<SpriteDraw>();
        anim = GetComponent<Animator>();
    }

   
    void Update()
    {
        anim.SetFloat("Angle", spr.ang); 
    }
}
