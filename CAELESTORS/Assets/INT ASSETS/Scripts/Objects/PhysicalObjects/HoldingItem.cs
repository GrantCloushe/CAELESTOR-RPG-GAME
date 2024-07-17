using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingItem : MonoBehaviour
{
    private CharacterController controller;
    CharacterInventory inventory;
    PlayerInterface playerInterface;

    private PhysicObjectController obj;
    private Transform holdingItem;
    private Block blockPhysix;

    [Header("ItemSettings")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private bool weaponInHand;
    [SerializeField] private SpriteDraw weaponSprite;
    [SerializeField] private SpriteDraw carryingItemSprite;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private Transform pivot;

    [Header("WeaponAttributes")]
    public float weaponStr;
    public float weaponSpeed;
    public float weaponRange;
    public float weaponKnockback;
    public float weaponVericality;

    float delay;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        obj = GetComponent<PhysicObjectController>();
        inventory = GetComponent<CharacterInventory>();
        playerInterface = GetComponent<PlayerInterface>();

        carryingItemSprite.spr.sprite = null;
    }
    
    void Update()
    {
        if(inventory.CurrentItem() != null)
        {
            weaponInHand = inventory.CurrentItem().isWeapon;
        }
        else
        {
            weaponInHand = false;
        }

        if(delay > 0)
        {
            delay--;
        }
  
        if (obj.isCarrying && blockPhysix != null)
        {
            holdingItem.position = pivot.position;
            holdingItem.rotation = pivot.rotation * Quaternion.Euler(0, 90, 0);

            if (obj.isCarrying && Input.GetButtonDown("Fire1") && delay <= 0)
            {
                obj.isCarrying = false;
                holdingItem = null;

                if (obj.isJumping)
                {
                    blockPhysix.GetImpulse(3, 3);
                    blockPhysix = null;
                }
                else
                {
                    blockPhysix.GetImpulse(0.25f, 1);
                    blockPhysix = null;
                }
            }
        }

        if (inventory.CurrentItem() != null)
        {
            if(inventory.CurrentItem().isCarrying && obj.currentAction == 0)
            {
                obj.isCarrying = true;
            }
            else
            {
                if (!inventory.CurrentItem().isCarrying)
                {
                    obj.isCarrying = false;
                    carryingItemSprite.spr.sprite = null;
                }
            }
            if (inventory.CurrentItem().isCarrying && obj.isCarrying)
            {
                carryingItemSprite.spr.sprite = inventory.CurrentItem().icon;
                blockPhysix = null;
            }
        }
        else if(inventory.CurrentItem() == null || inventory.CurrentItem().isCarrying == false)
        {
            if(blockPhysix == null)
            {
                obj.isCarrying = false;
            }

            carryingItemSprite.spr.sprite = null;
        }

        if(weaponInHand)
        {
            HoldingWeapon();
            weaponSprite.spr.sprite = inventory.CurrentItem().physicalSprite;
        }
        else
        {
            weaponSprite.spriteVisible = false;
            weaponStr = 0;
            weaponSpeed = 0;
            weaponKnockback = 0;
            weaponVericality = 0;
            weaponRange = 0;
        }

        if(obj.currentAction == 29)
        {
            weaponAnimator.enabled = false;
            pivot.localPosition = new Vector3(0, 0.5f, 0);
        }
        if(obj.currentAction == 0)
        {
            weaponAnimator.enabled = true;
            pivot.localPosition = new Vector3(0, 1f, 0);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.tag == "Block")
        {
            if(!obj.isCarrying && Input.GetButtonDown("Fire1"))
            {
                delay = 5;
                obj.isCarrying = true;
                holdingItem = col.GetComponent<Transform>();
                blockPhysix = col.GetComponent<Block>();
            }
        }
    }

    void HoldingWeapon()
    {
        weaponStr = inventory.CurrentItem().weaponStr;
        weaponSpeed = inventory.CurrentItem().weaponSpd;
        weaponKnockback = inventory.CurrentItem().weaponKnockback;
        weaponVericality = inventory.CurrentItem().weaponVerticality;
        weaponRange = inventory.CurrentItem().weaponRange;

        if (obj.isAttacking && !obj.isDodging && obj.currentAction != 17)
        {
            weaponSprite.spriteVisible = true;

            if(playerAnimator != null)
            {
                weaponAnimator.speed = playerAnimator.speed;
            }
        }
        else
        {
            weaponSprite.spriteVisible = false;
        }
    }
}
