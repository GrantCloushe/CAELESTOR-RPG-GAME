using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Transform proj;

    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private float gravity;
    [SerializeField] private float gravityForce;
    [SerializeField] private bool decreaseSpeed;
    [SerializeField] private float deceleration;
    [SerializeField] private bool haveGravity;
    [SerializeField] private bool haveLifeTime;


    void Start()
    {
        proj = GetComponent<Transform>();
    }

    void Update()
    {
        if(lifeTime > 0)
        {
            LifeTime();
        }
        else
        {
            if (haveLifeTime)
            {
                Destroy(gameObject);
            }
            else
            {
                LifeTime();

                if (speed < 0)
                {
                    Destroy(gameObject);
                }
            }

        }


        if(proj.position.y < 0)
        {
            Destroy(gameObject);
        }
    }

    void LifeTime()
    {
        lifeTime--;
        proj.position += transform.forward * Time.deltaTime * speed;
        proj.position += new Vector3(0, -gravity/100, 0);

        if (decreaseSpeed)
        {
            speed -= deceleration / 10;
        }

        if (haveGravity)
        {
            gravity += gravityForce / 10;
        }
    }

    public void GetHit()
    {
        Destroy(gameObject);
    }
}
