using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    Transform Object;

    [SerializeField] private float mass;
    public int blockID;
    public bool inBuilding;
    public bool destroyOnGround;
    [SerializeField] private bool onGround;
    [SerializeField] private bool disableGravity;
    [SerializeField] private float forwardImpulse;
    [SerializeField] private float solidLevel;
    [SerializeField] private Vector3 velocity;
    
    LayerMask collisionMask;
    shadowProjection shadow;


    void Start()
    {
        Object = GetComponent<Transform>();
        collisionMask = LayerMask.GetMask("Ground");
        shadow = GetComponentInChildren<shadowProjection>();

    }
 
    void Update()
    {

        if (!disableGravity)
        {
            Gravity();
        }

        if(shadow != null)
        {
            shadow.castShadow(Object, solidLevel);
        }
    }

    void Gravity()
    {
        float currentPosition = transform.position.y;

        if(forwardImpulse > 0)
        {
            Object.position += transform.forward * Time.deltaTime * (forwardImpulse * 5);
            forwardImpulse -= 0.05f;
        }

        if(velocity.y > 0)
        {
            velocity.y = velocity.y - 0.08f;
        }
        else
        {
            velocity.y = 0;
        }

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
            onGround = true;
            Object.position = new Vector3(Object.position.x, solidLevel + 0.5f, Object.position.z);

            if (destroyOnGround && forwardImpulse <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            onGround = false;
            Object.position -= new Vector3(0, (0.08f - velocity.y) + ((1 * mass)/100), 0);
        }



    }

    public void GetImpulse(float imp, float verticalVelocty)
    {
        forwardImpulse = imp;
        velocity.y = verticalVelocty/5;
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "CatapultLaunchZone")
        {
            Catapult cat = col.GetComponentInParent<Catapult>();

            cat.laucnhBlock = true;

            if (!cat.launch)
            {
                transform.position = cat.transform.position;
                transform.rotation = cat.transform.rotation;
            }
            
            if(cat.launchTimer > 40 && cat.launchTimer < 55 && cat.laucnhBlock)
            {
                transform.position = cat.transform.position + new Vector3(0, 2, 0);
                GetImpulse(cat.launchStr, cat.launchHeight);
            }
        }

        if(col.tag == "BuildingArea")
        {
            BuildingArea build = col.GetComponent<BuildingArea>();

            if(blockID == build.buildingBlockId && onGround)
            {
                if (!inBuilding)
                {
                    build.SetBox(true);
                    inBuilding = true; 
                }
                if (inBuilding)
                {
                    if (build.eatBlocks)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "BuildingArea")
        {
            BuildingArea build = col.GetComponent<BuildingArea>();

            if (blockID == build.buildingBlockId)
            {
                if (inBuilding)
                {
                    build.SetBox(false);
                    inBuilding = false;
                }
            }
        }
    }
}
