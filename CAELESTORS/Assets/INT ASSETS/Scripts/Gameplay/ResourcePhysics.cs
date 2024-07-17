using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePhysics : MonoBehaviour
{

    [SerializeField] private int resourseIndex;
    [SerializeField] private float mass;
    float solidLevel;
    Vector3 velocity;
    LayerMask collisionMask;
    shadowProjection shadow;

    void Start()
    {
        collisionMask = LayerMask.GetMask("Ground");
        shadow = GetComponentInChildren<shadowProjection>();
    }

    
    void Update()
    {
        Gravity();

        if (shadow != null)
        {
            shadow.castShadow(transform, solidLevel);
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

        if (currentPosition - solidLevel < 0.54f)
        {
            transform.position = new Vector3(transform.position.x, solidLevel + 0.5f, transform.position.z);

        }
        else
        {
            transform.position -= new Vector3(0, (0.08f - velocity.y) + ((1 * mass) / 1000), 0);
        }
    }

    public int Index()
    {
        return resourseIndex;
    }

    public int Count()
    {
        return Mathf.RoundToInt(mass/2);
    }
}
