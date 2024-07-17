using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0_Event3 : MonoBehaviour
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

    [SerializeField] private Transform cameraTool;
    [SerializeField] private Transform cameraPose;

    [SerializeField] private bool isActive;
    [SerializeField] private int cutscenePart;
    [SerializeField] private DialogueReference cutsceneDialogue;

    private float _timer;
    private bool cutsceneEnd;
    private bool setPosCamera;
    public int dialogueAction;

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
    }

    private void Start()
    {
        if(story.StoryChapter() == 0 && story.ChapterEvent() == 3)
        {
            if(!isActive && !cutsceneEnd)
            {
                fadeScreen.SetAlpha(1);
                fadeScreen.SwitchMode(false, 15);
                playerController.DisableControl();

                cam.SetViewport(cameraTool, 0, 0.5f, 0);
                cameraTool.position = cameraPose.position;
                cameraTool.rotation = cameraPose.rotation;
                TextTyping.EventAction += Actions;
                isActive = true;
            }
        }
    }
    void Update()
    {
        if(cutscenePart == 0 && isActive)
        {
            _timer++;

            if(_timer > 30)
            {
                dialogue.GetDialogueReference(cutsceneDialogue);
                playerController.DisableControl();
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = new Vector3(10.5f, 0.5f, 7f);
                box.BoxState(true);
                cutscenePart = 1;
            }
        }

        if(dialogueAction == 2 && isActive)
        {
            playerController.RestoreControl(0);
            player.GetComponent<CharacterController>().enabled = true;
            cam.SetViewport(player.transform, -7, 0.5f, 4);
            cutsceneEnd = true;
            isActive = false;
        }
    }

    void Actions(int action)
    {
        dialogueAction = action;
    }
}
