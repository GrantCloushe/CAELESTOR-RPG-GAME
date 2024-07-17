using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0_Event4 : MonoBehaviour
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

    [SerializeField] private EventTrigger eventTrig;
    [SerializeField] private bool isActive;
    [SerializeField] private int cutscenePart;
    [SerializeField] private DialogueReference cutsceneDialogue;

    private float _timer;
    private bool cutsceneEnd;
    bool stopTimer;
    private int dialogueAction;

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


    void Update()
    {
        if (story.StoryChapter() == 0 && story.ChapterEvent() == 4)
        {
            if (!isActive && !cutsceneEnd && eventTrig.Activation())
            {
                fadeScreen.SetAlpha(1);
                fadeScreen.SwitchMode(false, 15);
                cutscenePart = 0;
                isActive = true;
            }
        }

        if (isActive)
        {
            if (!stopTimer)
            {
                _timer++;
            }
            CutsceneParts();
        }
    }

    void CutsceneParts()
    {
        switch (cutscenePart)
        {
            case 0:
                TextTyping.EventAction += Actions;
                playerController.DisableControl();
                playerController.setPosition = new Vector3(124, 0.6f, 93);
                playerController.setPos = true;
                playerController.currentAction = -1;

                if (_timer > 4)
                {
                    cutscenePart = 1;
                    _timer = 0;
                    stopTimer = true;
                }

                break;

            case 1:

                playerController.Move(3, 90, 0, false);
                playerController.setPos = false;
                if(player.transform.position.x > 128.6)
                {
                    cutscenePart = 2;
                }
                break;

            case 2:
                playerController.Move(0, 90, 0, false);

                if(dialogueAction == 0)
                {
                    dialogue.GetDialogueReference(cutsceneDialogue);
                    box.BoxState(true);
                }

                if(dialogueAction == 1)
                {
                    cutscenePart = 3;
                    stopTimer = false;
                }
                break;
            case 3:
                if (_timer < 60)
                {
                    playerController.Move(3, 270, 0, false);
                }
                else
                {
                    playerController.Move(0, 270, 0, false);
                }

                if (dialogueAction == 2)
                {
                    cutscenePart = 4;
                    stopTimer = false;
                }
                break;
            case 4:
                playerController.RestoreControl(0);
                cutsceneEnd = true;
                isActive = false;
                break;
        }
    }

    void Actions(int action)
    {
        dialogueAction = action;
    }
}
