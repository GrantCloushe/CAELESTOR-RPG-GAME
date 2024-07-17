using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public Image itemSprite;
    public float durability;

    void Awake()
    {
        itemSprite = GetComponent<Image>();
    }

}
