using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseTrigger : MonoBehaviour
{
    public bool isActivated;
    [HideInInspector] public int _requiredId;
    [HideInInspector] public int _requiredState;

    [SerializeField] private Item requiredItem;
    [SerializeField] private int requiredID;
    [SerializeField] private int requiredState;


    [SerializeField] private bool setNewState;
    [SerializeField] private int setNewItemState;
    [SerializeField] private bool destroyItem;
    [SerializeField] private int requiredCount;
    [SerializeField] private int currentCount;
    
    void Update()
    {
        _requiredId = requiredID;
        _requiredState = requiredState;
    }

    public Item RequiredItm()
    {
        return requiredItem;
    }

    public void UseItem(Slot itm)
    {
        currentCount++;

        if (setNewState)
        {
            itm.itemState = setNewItemState;
        }

        if(currentCount >= requiredCount)
        {
            isActivated = true;
        }
    }
}
