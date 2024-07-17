using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0_Event2 : MonoBehaviour
{
    StoryProgress story;
    GameObject player;
    PhysicObjectController playerController;
    PlayerInterface playerInterface;
    PlayerInteraction playerInteraction;
    CameraControl cam;
    BlackScreen fadeScreen;
    TextBox box;
    TextTyping dialogue;

    [SerializeField] private GameObject floor;
    [SerializeField] private bool isActive;

    [SerializeField] private GameObject npc_Mother;
    [SerializeField] private int cutscenePart;
    [SerializeField] private DialogueReference cutsceneDialogue;
    [SerializeField] private Door kitchenDoor;

    private float _timer;
    private bool cutsceneEnd;

    NPC_Physics mother_physic;

    void Awake()
    {
        fadeScreen = FindObjectOfType<BlackScreen>();
        cam = Camera.main.GetComponent<CameraControl>();
        story = GetComponent<StoryProgress>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PhysicObjectController>();
        playerInterface = player.GetComponent<PlayerInterface>();
        playerInteraction = player.GetComponent<PlayerInteraction>();
        box = FindObjectOfType<TextBox>();
        dialogue = FindObjectOfType<TextTyping>();
        TextTyping.EventAction += CutsceneEnd;
    }

    void Update()
    {
        if (story.StoryChapter() == 0 && story.ChapterEvent() == 2)
        {
            if (floor.activeInHierarchy && !isActive && !cutsceneEnd)
            {
                if (playerController.currentAction == 29 && playerInterface.itemId == 2)
                {
                    playerController.DisableControl();
                    mother_physic = npc_Mother.GetComponent<NPC_Physics>();
                    cutscenePart = 0;
                    isActive = true;
                    npc_Mother.transform.localPosition = new Vector3(15.5f, -3.241f, 1);
                    dialogue.GetDialogueReference(cutsceneDialogue);

                }
            }
        }
        if (isActive)
        {
            CutscenePart();
        }
    }

    void CutscenePart()
    {
        switch (cutscenePart)
        {
            case 0:
                npc_Mother.transform.rotation = Quaternion.Euler(0, -90, 0);
                mother_physic.SetSpeed(4);
                if(npc_Mother.transform.localPosition.x < 13)
                {
                    npc_Mother.transform.localPosition = new Vector3(13f, -3.241f, 1);
                    cutscenePart = 1;
                }
                break;
            case 1:
                _timer++;
                mother_physic.action = 1;
                mother_physic.SetSpeed(0);
                npc_Mother.transform.rotation = Quaternion.Euler(0, 180, 0);

                if(_timer > 30)
                {
                    box.BoxState(true);
                    cutscenePart = 2;
                }
                break;
        }
    }

    void CutsceneEnd(int action)
    {
        if(action == 1 && !cutsceneEnd)
        {
            CharacterInventory inventory = player.GetComponent<CharacterInventory>();
            inventory.DestroyItem();
            player.GetComponent<PlayerInterface>().UpdateSlots();
            playerController.RestoreControl(0);
        }
    }
}
