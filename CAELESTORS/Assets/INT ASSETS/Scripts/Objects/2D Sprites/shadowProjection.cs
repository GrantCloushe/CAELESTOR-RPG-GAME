using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowProjection : MonoBehaviour
{
    LayerMask collisions;
    public Vector3 offset;

    void Start()
    {
        collisions = LayerMask.GetMask("Collisions");
    }


    void Update()
    {

    }

    public void castShadow(Transform parent, float solidGround)
    {
        Vector3 point = new Vector3(parent.position.x, solidGround, parent.position.z);
        transform.position = point + (offset + parent.forward * 0.025f);
    }
}
