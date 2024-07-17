using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [Header("Resourses")]
    [SerializeField] private int bronzeCount;
    [SerializeField] private int ferrumCount;
    [SerializeField] private int goldCount;
    [SerializeField] private int crytalCount;
    [SerializeField] private int arrowCount;

    [Header("Inventory")]
    [SerializeField] private Item[] items;
    private int selectedSlot = 0;

    public void AddResource(int ind, int count)
    {
        switch (ind)
        {
            case 0:
                bronzeCount += count;
                break;
            case 1:
                ferrumCount += count;
                break;
            case 2:
                goldCount += count;
                break;
            case 3:
                crytalCount += count;
                break;
            case 4:
                arrowCount += count;
                break;
        }
    }

    public int GetResource(int ind)
    {
        switch (ind)
        {
            case 0:
                if(bronzeCount > 999)
                {
                    bronzeCount = 999;
                }
                return bronzeCount;
            case 1:
                if (ferrumCount > 999)
                {
                    ferrumCount = 999;
                }
                return ferrumCount;
            case 2:
                if (goldCount > 999)
                {
                    goldCount = 999;
                }
                return goldCount;
            case 3:
                if (crytalCount > 999)
                {
                    crytalCount = 999;
                }
                return crytalCount;
            case 4:
                if (arrowCount > 999)
                {
                    arrowCount = 999;
                }
                return arrowCount;
            default:
                return 0;
                
        }
    }
    
    public int InventoryLenght()
    {
        return items.Length;
    }

    public Item CurrentItem()
    {
        return items[selectedSlot];
    }

    public Item CurrentItem(int i)
    {
        return items[i];
    }

    public void GiveItem(Item itm, int slot)
    {
        items[slot] = itm;
    }

    public void SelectSlot(int select)
    {
        selectedSlot = select;
    }

    public void DestroyItem()
    {
        items[selectedSlot] = null;
    }
    public void DestroyItem(int slot)
    {
        items[slot] = null;
    }
    public void DestroyAll()
    {

    }
}
