using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicItem : MonoBehaviour
{
    [Header("ItemValues")]
    public Item item;
    [SerializeField] private SpriteRenderer itemSprite;

    [Header("PhysicValues")]
    [SerializeField] private float gravityForce;
    [SerializeField] private bool disableGravity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float forwardImpulse;
    [SerializeField] private float verticalImpulse;
    public bool isHurtable;
    public int state;

    private LayerMask collisionMask;
    private shadowProjection shadow;
    private float solidLevel;

    void Awake()
    {
        collisionMask = LayerMask.GetMask("Ground");
        shadow = GetComponentInChildren<shadowProjection>();
        itemSprite.sprite = item.physicalSprite;
    }

    void Update()
    {

        transform.position += transform.forward * Time.deltaTime * forwardImpulse / 10;
        transform.position += new Vector3(0, verticalImpulse / 10, 0); 

        if(forwardImpulse > 0)
        {
            forwardImpulse -= (gravityForce / 10) + (gravityForce * System.Convert.ToInt16(isGrounded) / 5);
        }
        else
        {
            forwardImpulse = 0;
        }

        if(verticalImpulse > 0)
        {
            verticalImpulse -= gravityForce / 10;
        }
        else
        {
            verticalImpulse = 0;
        }

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

        if (!isGrounded)
        {
            transform.position -= new Vector3(0, gravityForce / 100, 0);
        }

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
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }

    public void GetImpulse(float _forward, float _vertical)
    {
        forwardImpulse = _forward;
        verticalImpulse = _vertical;
    }
}
