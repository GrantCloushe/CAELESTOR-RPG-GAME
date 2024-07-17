using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPreset : MonoBehaviour
{
    SpriteRenderer sprRender;
    SpriteDraw sprDraw;
    private PhysicObjectController obj;
    private Animator anim;
    private float resetDelay;
    [SerializeField] private bool setAngle;
    [SerializeField] private bool setHitColor = true;

    void Start()
    {
        sprDraw = GetComponent<SpriteDraw>();
        sprRender = GetComponent<SpriteRenderer>();
        obj = GetComponentInParent<PhysicObjectController>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if(obj.currentSpeed < 1 && obj.currentSpeed > -1)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
            AnimationSide();
        }

        anim.SetBool("isJumping", obj.isJumping);
        anim.SetFloat("Knockback", obj._knockback);
        anim.SetInteger("Action", obj.currentAction);

        if (setAngle)
        {
            anim.SetFloat("Angle", sprDraw.ang);
        }

        if (setHitColor)
        {
            HitStun();
        }

        ResetDelays();

        if (obj.isAttacking)
        {
            anim.speed = obj.GetModifier(1) * 0.25f;
        }
        else
        {
            anim.speed = 1;
        }
    }

    public void AnimationAction(int setAction)
    {
        resetDelay = 0;
        obj.InputsActionControl(setAction);
    }

    public void ActionFrames(int action)
    {
        switch (action)
        {
            case 1:
                obj.isAttacking = true;
                break;

            case 2:
                obj.isAttacking = false;
                break;
        }
    }

    void AnimationSide()
    {
        if (obj.currentSpeed > 1)
        {
            anim.SetInteger("Side", 1);
        }
        else if (obj.currentSpeed < 0)
        {
            anim.SetInteger("Side", -1);
        }
    }

    void HitStun()
    {
        if (obj.isStunning)
        {
            if (sprRender.color.r < 0.45f)
            {
                sprRender.color = new Color(0.45f, 0.25f, 0);
            }
            else
            {
                sprRender.color = new Color(1 - (obj._damageTaken / 100), 0.5f - (obj._damageTaken / 100), 0);
            }
        }
        else
        {
            sprRender.color = new Color(1, 1, 1);
        }
    }

    void ResetDelays()
    {

        if (obj.currentAction == 1)
        {
            resetDelay++;

            if (resetDelay > (30 + (10 - obj.GetModifier(1)) * 2))
            {
                resetDelay = 0;
                obj.currentAction = 0;
            }
        }

        if (obj.currentAction == 2)
        {
            resetDelay++;

            if (resetDelay > (50 + (10 - obj.GetModifier(1)) * 2))
            {
                resetDelay = 0;
                obj.currentAction = 0;
            }
        }
    }
}
