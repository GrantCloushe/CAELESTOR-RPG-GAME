using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Slot : MonoBehaviour
{
    Image slotSprite;
    SlotItem itemInSlot;

    [SerializeField] private Sprite[] slotStates;
    public int itemState;
    [SerializeField] private bool isSelected;
    private bool newState;

    [SerializeField] private int itemId;

    void Awake()
    {
        slotSprite = GetComponent<Image>();
        itemInSlot = GetComponentInChildren<SlotItem>();

        SlotState();
    }

    void Update()
    {
        if(newState != isSelected)
        {
            SlotState();
        }
    }

    public void SlotState(bool select = false)
    {
        isSelected = select;
        slotSprite.sprite = slotStates[Convert.ToInt16(isSelected)];
        newState = isSelected;
    }

    public void HaveItem(Item itm)
    {
        if(itm != null)
        {
            itemId = itm.itemId;
            if(itemState == 0)
            {
                itemInSlot.itemSprite.sprite = itm.icon;
            }
            else
            {
                itemInSlot.itemSprite.sprite = itm.statesIcon[itemState];
            }
        }
        else
        {
            itemId = 0;
            itemInSlot.itemSprite.sprite = null;
        }
    }

    public bool SlotIsEmpty()
    {
        if(itemId == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int ItemId()
    {
        return itemId;
    }

}
