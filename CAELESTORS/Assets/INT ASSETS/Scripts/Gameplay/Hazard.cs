using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private Animator anim;
    public bool active;
    [SerializeField] private bool changeStates;
    [SerializeField] private float changeStateTimer;
    public float damage;
    public float knockback;

    Projectile proj; 

    void Start()
    {
        if (changeStates)
        {
            anim = GetComponentInChildren<Animator>();
            InvokeRepeating("ChangeState", changeStateTimer, changeStateTimer);
        }
        else
        {
            active = true;
        }
    }

    void ChangeState()
    {
        active = !active;
        anim.SetBool("State", active);
    }

    public bool isProjectile()
    {
        proj = GetComponentInParent<Projectile>();

        if(proj != null)
        {
            proj.GetHit();
            return true;
        }
        else
        {
            return false;
        }
    }
}
