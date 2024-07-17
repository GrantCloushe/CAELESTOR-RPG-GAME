using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerInterface : MonoBehaviour
{
    PhysicObjectController player;
    CharacterInventory inventory;

    [SerializeField] private List<Resource> resources;
    [SerializeField] private List<Stat> stats;

    [SerializeField] private Text arrowCounter;
    [SerializeField] private Text _damage;

    [SerializeField] private Slot[] playerInventorySlots;

    private int selectedSlot = 0;
    public int itemId;

    void Start()
    {
        player = GetComponent<PhysicObjectController>();
        inventory = GetComponent<CharacterInventory>();

        selectedSlot = 0;
        UpdateSlots();
    }

    void Update()
    {
        if(arrowCounter != null)
        {
            arrowCounter.text = inventory.GetResource(4).ToString();
        }

        float damage = Mathf.RoundToInt(player._damageTaken);

        for (int i = 0; i < resources.Count; i++)
        {
            if(resources[i].counter != null)
            {
                resources[i].counter.text = inventory.GetResource(i).ToString();
            }
        }

        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].counter != null)
            {
                stats[i].counter.text = player.GetModifier(i).ToString();
            }
        }

        _damage.text = damage.ToString();

        if (player.IsPlayable())
        {
            if (Input.GetKeyDown("1"))
            {
                ChangeSlot(0);
            }
            if (Input.GetKeyDown("2"))
            {
                ChangeSlot(1);
            }
            if (Input.GetKeyDown("3"))
            {
                ChangeSlot(2);
            }
            if (Input.GetKeyDown("4"))
            {
                ChangeSlot(3);
            }
            if (Input.GetKeyDown("q"))
            {
               ThrowItem();
            }

        }
    }

    public Slot GetCurrentSlot()
    {
        return playerInventorySlots[selectedSlot];
    }

    public void UpdateSlots()
    {
        for (int i = 0; i < playerInventorySlots.Length; i++)
        {
            if(i != selectedSlot)
            {
                playerInventorySlots[i].SlotState(false);
            }

            if(inventory.CurrentItem(i) != null)
            {
                playerInventorySlots[i].HaveItem(inventory.CurrentItem(i));
            }
            else
            {
                playerInventorySlots[i].HaveItem(null);
            }
        }
        playerInventorySlots[selectedSlot].SlotState(true);
        itemId = playerInventorySlots[selectedSlot].ItemId();

        inventory.SelectSlot(selectedSlot);
    }

    void ChangeSlot(int set)
    {
        selectedSlot = set;

        if(selectedSlot > playerInventorySlots.Length - 1)
        {
            selectedSlot = playerInventorySlots.Length - 1;
        }
        if(selectedSlot < 0)
        {
            selectedSlot = 0;
        }
        UpdateSlots();
    }

    void ThrowItem()
    {
        GameObject throwObject = inventory.CurrentItem().physicalPrefab;
        PhysicItem itemPhysic = throwObject.GetComponent<PhysicItem>();
        throwObject.transform.position = new Vector3(transform.position.x, player.transform.position.y, player.transform.position.z);
        throwObject.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        itemPhysic.GetImpulse(64 + player.currentSpeed, 2 + player.currentSpeed / 15);
        itemPhysic.state = GetCurrentSlot().itemState;
        Instantiate(throwObject);

        inventory.DestroyItem();
        UpdateSlots();
    }

    [System.Serializable]
    public struct Resource
    {
        public string resourceName;
        public Text counter;
    }

    [System.Serializable]
    public struct Stat
    {
        public string statName;
        public Text counter;
    }
}
