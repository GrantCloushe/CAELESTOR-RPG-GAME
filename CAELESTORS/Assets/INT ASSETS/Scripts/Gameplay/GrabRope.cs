using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRope : MonoBehaviour
{
    RopePivot pivotCentre;

    void Start()
    {
        pivotCentre = GetComponentInParent<RopePivot>();
    }

    public void Grab(float amp)
    {
        pivotCentre.GetAmplitude(amp * 2);
    }
}
