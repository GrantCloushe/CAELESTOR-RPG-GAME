using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStatesMachine : MonoBehaviour
{
    [SerializeField] private int currentState;
    [SerializeField] private BoxCollider[] statesTrigger;
    private int newState;

    void Awake()
    {
        newState = currentState;
        UpdateStates();
    }

    private void Update()
    {
        if(currentState != newState)
        { 
            currentState = newState;
            UpdateStates();
        }
    }

    private void UpdateStates()
    {
        for(int i = 0; i < statesTrigger.Length; i++)
        {
            if( i != currentState)
            {
                if (statesTrigger[i].isTrigger)
                {
                    statesTrigger[i].enabled = false;
                }
            }
            else
            {
                if (statesTrigger[i].isTrigger)
                {
                    statesTrigger[i].enabled = true;
                }
            }
        }
    }

    public void SetNewState(int newSetState)
    {
        newState = newSetState;
    }
}
