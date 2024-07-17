using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Physics : MonoBehaviour
{
    [SerializeField] private float gravityForce;
    [SerializeField] private bool disableGravity;
    [SerializeField] private float speed;
    public int action;

    private LayerMask collisionMask;
    private shadowProjection shadow;
    private float solidLevel;

    void Awake()
    {
        collisionMask = LayerMask.GetMask("Ground");
        shadow = GetComponentInChildren<shadowProjection>();
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed / 10;

        if (shadow != null)
        {
            shadow.castShadow(transform, solidLevel);
        }

        if (!disableGravity)
        {
            Gravity();
        }
    }

    void Gravity()
    {
        float currentPosition = transform.position.y;

        RaycastHit ray;

        if (Physics.Raycast(transform.position, Vector3.down, out ray, 1000f, collisionMask))
        {
            float currentSolidLevel = ray.point.y;
            solidLevel = Mathf.Round(currentSolidLevel * 100.0f) * 0.01f;
        }
        else
        {
            solidLevel = -9999f;
        }

        if (currentPosition - solidLevel > 0.59f)
        {
            transform.position += new Vector3(0, gravityForce/100, 0);
        }


    }

    public bool isWalking()
    {
        if(speed >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public float Speed()
    {
        return speed;
    }
    public void SetSpeed(float _setSpeed)
    {
        speed = _setSpeed;
    }
}
