using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0_Event1 : MonoBehaviour
{
    StoryProgress story;
    GameObject player;
    PhysicObjectController playerController;
    CameraControl cam;
    BlackScreen fadeScreen;
    TextBox box;
    TextTyping dialogue;

    [SerializeField] private Transform cameraTool;
    [SerializeField] private GameObject floor;
    [SerializeField] private bool isActive;
    [SerializeField] private GameObject npc_Draidek;
    [SerializeField] private GameObject npc_Kai;
    [SerializeField] private GameObject npc_Mother;
    [SerializeField] private int cutscenePart;
    [SerializeField] private DialogueReference cutsceneDialogue;
    [SerializeField] private Door kitchenDoor;

    private float _timer;
    private bool cutsceneEnd;

    NPC_Physics draidek_physic;
    NPC_Physics kai_physic;
    NPC_Physics mother_physic;

    void Awake()
    {
        fadeScreen = FindObjectOfType<BlackScreen>();
        cam = Camera.main.GetComponent<CameraControl>();
        story = GetComponent<StoryProgress>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PhysicObjectController>();
        box = FindObjectOfType<TextBox>();
        dialogue = FindObjectOfType<TextTyping>();
        TextTyping.EventAction += Action;
    }

    void Update()
    {
        if(story.StoryChapter() == 0 && story.ChapterEvent() == 1)
        {
            if (floor.activeInHierarchy && !isActive && !cutsceneEnd)
            {
                npc_Kai.SetActive(true);
                draidek_physic = npc_Draidek.GetComponent<NPC_Physics>();
                kai_physic = npc_Kai.GetComponent<NPC_Physics>();
                mother_physic = npc_Mother.GetComponent<NPC_Physics>();

                StartCoroutine(Cutscene());
                cutscenePart = 0;
                cam.SetViewport(null, 0, 0, 0);
                cam.SetAngle(0, 0, 0);
                isActive = true;
                dialogue.GetDialogueReference(cutsceneDialogue);
            }

            if (isActive)
            {
                CutscenePart();
            }

            if (cutsceneEnd)
            {
                npc_Draidek.SetActive(false);
            }
        }
    }

    IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(1.25f);
        cutscenePart = 1;
        yield return new WaitForSeconds(3f);
        cutscenePart = 2;
        yield return new WaitForSeconds(2.5f);
        cutscenePart = 3;
        yield return new WaitForSeconds(5.25f);
        cutscenePart = 4;
    }

    void CutscenePart()
    {
        switch (cutscenePart)
        {
            case 0:
                player.SetActive(false);
                npc_Kai.transform.localPosition = new Vector3(11.5f, -3.2f, -4.711f);
                npc_Draidek.transform.localPosition = new Vector3(14.5f, -3.2f, 1);
                npc_Mother.transform.localPosition = new Vector3(15.5f, -3.2f, 1);

                cameraTool.position = new Vector3(18, 0.5f, -8.75f);
                cam.SetViewport(cameraTool, 0, 0.5f, 8);
                break;

            case 1:
                cameraTool.position += new Vector3(0, 0, -0.02f);

                npc_Kai.transform.rotation = Quaternion.Euler(0, 90, 0);
                kai_physic.SetSpeed(6);
                break;

            case 3:
                npc_Kai.transform.rotation = Quaternion.Euler(0, 0, 0);
                cameraTool.position = npc_Kai.transform.position + new Vector3(0, 0.5f, -5.9999f);
                kai_physic.SetSpeed(8);
                break;

            case 4:
                kai_physic.SetSpeed(0);
                box.BoxState(true);
                break;
            case 5:
                npc_Draidek.transform.rotation = Quaternion.Euler(0, 0, 0);
                draidek_physic.SetSpeed(1.8f);
                break;
            case 6:
                npc_Draidek.transform.rotation = Quaternion.Euler(0, 0, 0);
                draidek_physic.SetSpeed(0);
                break;
            case 7:
                draidek_physic.SetSpeed(6);
                kitchenDoor.Interaction(false);
                cameraTool.position = npc_Kai.transform.position + new Vector3(0, 0.5f, -5.9999f);
                cameraTool.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case 8:
                _timer++;
                kitchenDoor.CloseDoor();

                if(_timer > 240)
                {
                    player.SetActive(true);
                    player.transform.position = npc_Kai.transform.position;
                    player.transform.rotation = npc_Kai.transform.rotation;
                    playerController.RestoreControl(0);
                    npc_Kai.SetActive(false);
                    cam.SetViewport(player.transform, -7, 0.5f, 5);
                    isActive = false;
                    cutsceneEnd = true;
                }
                break;
        }
    }

    void Action(int action)
    {
        if (isActive)
        {
            switch (action)
            {
                case 1:
                    cutscenePart = 5;
                    break;
                case 2:
                    cutscenePart = 6;
                    break;
                case 3:
                    cutscenePart = 7;
                    break;
                case 4:
                    cutscenePart = 8;
                    break;
            }
        }
    }
}
