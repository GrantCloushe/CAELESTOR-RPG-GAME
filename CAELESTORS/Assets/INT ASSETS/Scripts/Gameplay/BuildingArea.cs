using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingArea : MonoBehaviour
{
    public int buildingBlockId;
    public bool eatBlocks;
    [SerializeField] private GameObject building;
    [SerializeField] private int counter;
    [SerializeField] private GameObject text;
    [SerializeField] private Sprite[] counterImage;
    [SerializeField] private SpriteRenderer counterSprite;

    void Update()
    {
        counterSprite.sprite = counterImage[counter];

        if(building != null)
        {
            if (counter == 0)
            {
                building.SetActive(true);
            }
            else
            {
                building.SetActive(false);
            }
        }

        BuildingState();
    }

    public bool BuildingState()
    {
        if(counter == 0)
        {
            eatBlocks = true;
            text.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShowText()
    {
        text.SetActive(true);
    }

    public void HideText()
    {
        text.SetActive(false);
    }

    public void SetBox(bool state)
    {
        if (state)
        {
            counter--;
        }
        else
        {
            counter++;
        }
    }
}
