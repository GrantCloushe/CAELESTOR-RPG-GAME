using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]

public class Item : ScriptableObject
{
    [Header("Item Info")]
    public Sprite icon;
    public int itemId;
    public string itemName = "new Item";

    [Header("Trade Values")]
    public int coast;

    [Header("Physic Values")]
    public Sprite physicalSprite;
    public GameObject physicalPrefab;
    public bool destructible;
    public bool isCarrying;
    public int state;
    public Sprite[] statesIcon;

    [Header("Weapon Values")]
    public bool isWeapon;
    public float weaponStr;
    public float weaponSpd;
    public float weaponKnockback;
    public float weaponVerticality;
    public float weaponRange;
}
