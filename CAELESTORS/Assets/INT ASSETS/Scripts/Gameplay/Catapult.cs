using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    Transform catapult;
    Animator anim;
    [SerializeField] private float rotation;
    public float launchTimer;
    public bool control;
    public bool launch;
    public bool laucnhBlock;
    public float launchStr = 8;
    public float launchHeight = 3;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        catapult = GetComponent<Transform>();
        rotation = catapult.eulerAngles.y;
    }

    
    void Update()
    {
        if(transform.position.y < -3)
        {
            Destroy(gameObject);
        }
        anim.SetBool("Launch", launch);
        catapult.rotation = Quaternion.Euler(0, rotation, 0);

        if(rotation > 360)
        {
            rotation = 0;
        }

        if(rotation < 0)
        {
            rotation = 359;
        }

        if (launch)
        {
            launchTimer++;

            if(launchTimer > 60)
            {
                launchTimer = 0;
                launch = false;
                laucnhBlock = false;
            }
        }

    }

    public void Rotate(float side)
    {
        rotation += side;
    }
}
