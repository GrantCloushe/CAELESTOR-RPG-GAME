using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private int activationMethod;
    private bool activate;

    public void Activate()
    {
        activate = true; 
    }

    public bool Activation()
    {
        return activate;
    }
}
