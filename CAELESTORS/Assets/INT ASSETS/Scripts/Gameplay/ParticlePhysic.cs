using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePhysic : MonoBehaviour
{
    SpriteRenderer particleImage;
    Transform partT;

    [SerializeField] private Vector3 velocity;
    private float deceleration = 2f;
    private float forwardAcceleration = 0.25f;
    [SerializeField] private Transform target;
    [SerializeField] private float followDistance;
    [SerializeField] private bool destroyOnDistance;
    [SerializeField] private bool haveGravity;
    [SerializeField] private float gravityForce;
    [SerializeField] private float groundLevel = 0;
    [SerializeField] private bool destroyOnGround;
    [SerializeField] private bool haveLifeTime;
    [SerializeField] private float lifeTime;

    private bool initiated;

    private void Awake()
    {
        partT = GetComponent<Transform>();
        particleImage = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        transform.position += velocity/50;

        if(target != null && initiated)
        {
            transform.LookAt(target);
            float dist = Vector3.Distance(partT.position, target.position);

            if(dist > followDistance)
            {
               transform.position += transform.forward * Time.deltaTime * forwardAcceleration / 50;
            }
            else
            {
                if (destroyOnDistance)
                {
                    Destroy(gameObject);
                }
            }
        }

        velocity.x = NewVelocty(velocity.x);
        velocity.y = NewVelocty(velocity.y) - GravityForce(gravityForce);
        velocity.z = NewVelocty(velocity.z);

        if (haveLifeTime)
        {
            lifeTime--;
            if(lifeTime < 0)
            {
                Destroy(gameObject);
            }
        }

    }

    public void TargetSetup(Transform setTarget, float forwardSpeed = 0.25f, float setFollowDist = 0.75f, bool destroyDist = true)
    {
        target = setTarget;
        followDistance = setFollowDist;
        destroyOnDistance = destroyDist;
    }

    public void GravityParametres(bool setGravity, float setForce, float setGround, bool setDestroyOnGround)
    {
        haveGravity = setGravity;
        gravityForce = setForce;
        groundLevel = setGround;
        destroyOnGround = setDestroyOnGround;
    }

    public void SetVelocity(Vector3 set)
    {
        if (!initiated)
        {
            velocity = set;
        }
    }

    public void Initiated()
    {
        initiated = true;
    }
    
    float NewVelocty(float currentVelocity = 0)
    {
        if (currentVelocity> 0)
        {
            currentVelocity -= deceleration / 10;
        }

        return currentVelocity;
    }

    float GravityForce(float gravityForce)
    {
        if (!haveGravity)
        {
            return 0f;
        }
        else
        {
            if(partT.position.y > groundLevel)
            {
                return gravityForce / 500;
            }
            else
            {
                if (destroyOnGround)
                {
                    Destroy(gameObject);
                }
                return 0;

            }
        }

    }
}
