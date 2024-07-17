using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInteraction : MonoBehaviour
{
    CharacterController controller;
    PhysicObjectController player;
    CharacterInventory inventory;
    PlayerInterface playerInterface;

    CameraControl cam;
    TextBox box;
    TextTyping text;
    GameOverResult result;
    TaskPapperUI task;

    [SerializeField] private Transform[] spawnPoints;
    public static int goFrom;
    [SerializeField] private GameObject actionText;
    [SerializeField] private GameObject interactionText;
    [SerializeField] private DialogueReference generalDialogue;

    private bool sitting;
    private int currentDistance;
    private bool action_button;
    private bool interaction_button;
    float delay;
    bool itemTaken;

    private void Awake()
    {
        player = GetComponent<PhysicObjectController>();
        controller = GetComponent<CharacterController>();
        inventory = GetComponent<CharacterInventory>();
        playerInterface = GetComponent<PlayerInterface>();

        cam = Camera.main.GetComponent<CameraControl>();
        box = FindObjectOfType<TextBox>();
        text = FindObjectOfType<TextTyping>();
        result = FindObjectOfType<GameOverResult>();
        task = FindObjectOfType<TaskPapperUI>();

        GameOverResult.ResultEnd += Spawn;
        transform.position = spawnPoints[goFrom].position;
    }

    void Start()
    {
        if(task != null)
        {
            task.TaskAppear();
        }

        currentDistance = -8;
        actionText.SetActive(false);
        interactionText.SetActive(false);

        generalDialogue = GetComponentInChildren<DialogueReference>();
    }
    
    void Update()
    {
        if (player.IsPlayable())
        {
            action_button = Input.GetButtonDown("Fire1");
            interaction_button = Input.GetButtonDown("Fire2");
        }
        else
        {
            action_button = false;
            interaction_button = false;
        }

        if(box.BoxAnimationState() == 1)
        {
            player.DisableControl();
        }
        if(box.BoxAnimationState() == 2)
        {
            player.RestoreControl(0);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "NPC")
        {
            actionText.SetActive(true);
        }

        if (col.tag == "Stairs")
        {
            actionText.SetActive(true);
        }

        if (col.tag == "SceneJump" && col.GetComponent<JumpToSceneTrigger>().NeedPress())
        {
            actionText.SetActive(true);
        }

        if (col.tag == "Seat")
        {
            actionText.SetActive(true);
        }

        if(col.tag == "ItemUse")
        {
            ItemUseTrigger use = col.GetComponent<ItemUseTrigger>();
            if (!use.isActivated)
            {
                if (inventory.CurrentItem() == use.RequiredItm() || playerInterface.itemId == use._requiredId)
                {
                    if (playerInterface.GetCurrentSlot().itemState == use._requiredState)
                    {
                        actionText.SetActive(true);
                    }
                }
            }
        }

        if (col.tag == "Container")
        {
            Container cont = col.GetComponent<Container>();

            if (!cont.requireItem && cont.count > 0)
            {
                interactionText.SetActive(true);
            }
            if (cont.requireItem)
            {
                if (playerInterface.itemId == cont.requiredItemId && cont.count > 0)
                {
                    interactionText.SetActive(true);
                }
            }

        }

        if(col.GetComponent<ChangeBackground>() != null)
        {
            col.GetComponent<ChangeBackground>().ChangeBG();
        }
        
        if (col.tag == "Item")
        {
            actionText.SetActive(true);
        }

        if(col.GetComponent<EventTrigger>() != null)
        {
            col.GetComponent<EventTrigger>().Activate();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "NPC")
        {
            actionText.SetActive(false);
            player.RestoreControl(0);
            currentDistance = -8;
            cam.ChangeDistance(currentDistance);
        }

        if(col.tag == "SceneJump")
        {
            actionText.SetActive(false);
        }

        if (col.tag == "Stairs")
        {
            actionText.SetActive(false);
        }

        if (col.tag == "Seat")
        {
            actionText.SetActive(false);
        }

        if (col.tag == "Container")
        {
            interactionText.SetActive(false);
        }

        if (col.tag == "ItemUse")
        {
            actionText.SetActive(false);
        }

        if (col.tag == "Item")
        {
            actionText.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "NPC")
        {
            if (col.GetComponent<BringItemQuest>() != null)
            {
                BringItemQuest quest = col.GetComponent<BringItemQuest>();
                Debug.Log("Quest");

                if (quest.CurrentQuestState == 1 && action_button)
                {
                    quest.CheckItem(inventory.CurrentItem(), playerInterface.GetCurrentSlot().itemState);
                    if (!quest.wrongItem)
                    {
                        return;
                    }
                }
                if (quest.CurrentQuestState == 2)
                {
                    Debug.Log("Квест выполнен");
                    inventory.DestroyItem();
                    playerInterface.UpdateSlots();
                    text.ResetDialogueLine();
                    box.BoxState(true);
                    text.GetDialogueReference(quest.completedDialogue);
                    quest.EndQuest();
                }
            }

            if (action_button )
            {            
                if (col.GetComponent<DialogueReference>() != null)
                {
                    actionText.SetActive(false);
                    //text.ResetDialogueLine();
                    box.BoxState(true);
                    text.GetDialogueReference(col.GetComponent<DialogueReference>());
                    GetComponentInChildren<CharacterManager>().CharacterVisible(false);
                    transform.LookAt(new Vector3(col.transform.position.x, transform.position.y, col.transform.position.z));

                    if (currentDistance < -4)
                    {
                        currentDistance += 2;
                        cam.ChangeDistance(currentDistance);
                    }
                }

            }
        }

        if(col.tag == "NPC" && box.BoxAnimationState() == 2)
        {
            actionText.SetActive(true);
            if(currentDistance > -8)
            {
                GetComponentInChildren<CharacterManager>().CharacterVisible(true);
                currentDistance -= 1;
                cam.ChangeDistance(currentDistance);
            }
        }

        if(col.tag == "SceneJump")
        {
            JumpToSceneTrigger jumpTrigger = col.GetComponent<JumpToSceneTrigger>();

            if (!jumpTrigger.NeedPress())
            {
                jumpTrigger.Activate();
                goFrom = jumpTrigger.goTo;

                player.DisableControl();
            }
            else
            {
                if (action_button)
                {
                    jumpTrigger.Activate();
                    goFrom = jumpTrigger.goTo;
                    player.DisableControl();
                }
            }
        }

        if(col.tag == "Stairs" && action_button)
        {
            col.GetComponent<StairsTrigger>().ChangeFloor();
            actionText.SetActive(false);

            if (col.GetComponent<StairsTrigger>().locked)
            {
                text.ResetDialogueLine();
                generalDialogue.startLine = 0;
                generalDialogue.endLine = 1;
                text.GetDialogueReference(generalDialogue);

                box.BoxState(true);
            }
        }

        if(col.tag == "Stairs")
        {
            StairsTrigger elevator = col.GetComponent<StairsTrigger>();

            if (elevator.IsElevating())
            {
                GetComponent<HoldingItem>().enabled = false;
                player.isCarrying = false;
                player.currentAction = 30;

                player.parentPivot = elevator.elevatingPivot;
                player.transform.rotation = elevator.elevatingPivot.rotation;
            }
            else
            {
                if(player.currentAction == 30)
                {
                    GetComponent<HoldingItem>().enabled = true;
                    player.currentAction = 0;
                    player.disableGravity = false;
                    player.parentPivot = null;
                }
            }
        }

        if (col.tag == "Seat" && action_button)
        {
            sitting = !sitting;
            Sitting(col.transform);
        }

        if (col.tag == "Container")
        {
            Container cont = col.GetComponent<Container>();

            if (!cont.requireItem && interaction_button && cont.count > 0)
            {
                cont.count--;
                AddItems(cont.giveItems);
                interactionText.SetActive(false);
                playerInterface.UpdateSlots();
            }
            if (cont.requireItem)
            {
                if(playerInterface.itemId == cont.requiredItemId && interaction_button && cont.count > 0) 
                {
                    if (cont.destroyIteminHand)
                    {
                        inventory.DestroyItem();
                    }
                    playerInterface.UpdateSlots();
                    cont.count--;
                    AddItems(cont.giveItems);
                    interactionText.SetActive(false);
                    playerInterface.UpdateSlots();
                }
            }
        }

        if (col.tag == "ItemUse")
        {
            ItemUseTrigger use = col.GetComponent<ItemUseTrigger>();

            if (inventory.CurrentItem() == use.RequiredItm() || playerInterface.itemId == use._requiredId)
            {
                if(playerInterface.GetCurrentSlot().itemState == use._requiredState)
                {
                    if (action_button)
                    {
                        use.UseItem(playerInterface.GetCurrentSlot());
                        actionText.SetActive(false);
                        playerInterface.UpdateSlots();
                    }
                }
            }
        }

        if(col.tag == "Item")
        {
            if (action_button)
            {
                PhysicItem item = col.GetComponent<PhysicItem>();
                AddItem(item.item);
                playerInterface.UpdateSlots();
                actionText.SetActive(false);

                if (itemTaken)
                {
                    Destroy(col.gameObject);
                    itemTaken = false;
                }
            }
        }
    }

    public void ShowResult(int resultID)
    {
        if(resultID == 0)
        {
            result.ShowResult();
        }
    }

    public void Spawn()
    {
        transform.position = spawnPoints[goFrom].position;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }

    public void Spawn(int setSpawnPoint)
    {
        gameObject.GetComponent<CharacterController>().enabled = false;
        transform.position = spawnPoints[setSpawnPoint].position;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }

    void Sitting(Transform sittingPivot)
    {
        if (sitting)
        {
            player.isCarrying = false;
            player.currentAction = 29;
            player.parentPivot = sittingPivot.transform;
            actionText.SetActive(false);
        }
        else
        {
            player.currentAction = 0;
            player.parentPivot = null;
        }
    }

    void AddItem(Item give)
    {
        int emptySlot = 0;
 
        while (emptySlot < inventory.InventoryLenght())
        {
            if (inventory.CurrentItem(emptySlot) == null)
            {
                inventory.GiveItem(give, emptySlot);
                itemTaken = true;
                Debug.Log("EmptySlot Is" + emptySlot);

                emptySlot++;
                return;
            }
            else
            {
                emptySlot++;
            }
        }
        itemTaken = false;
        return;
    }

    void AddItems(Item[] give)
    {
        int emptySlot = 0;
        int addItemFrom = 0;

        Debug.Log(give.Length);

        while(emptySlot < inventory.InventoryLenght())
        {
            if(inventory.CurrentItem(emptySlot) == null)
            {
                inventory.GiveItem(give[addItemFrom], emptySlot);
                Debug.Log("AddItemID is" + addItemFrom);
                Debug.Log("EmptySlot Is" + emptySlot);

                emptySlot++;
                addItemFrom++;

                if(addItemFrom > give.Length - 1)
                {
                    Debug.Log("GiveItemsisEnd");
                    return;
                }
            }
            else
            {
                emptySlot++;
            }
        }

        return;
    }
}
