using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0_Event0 : MonoBehaviour
{
    StoryProgress story;
    GameObject player;
    PhysicObjectController playerController;
    CameraControl cam;
    BlackScreen fadeScreen;
    TextBox box;
    TextTyping dialogue;

    [SerializeField] private Transform cameraTool;
    [SerializeField] private bool isActive;
    [SerializeField] private int cutscenePart;
    [SerializeField] private GameObject bed;
    [SerializeField] private DialogueReference cutsceneDialogue;
    private int _timer;

    private void Awake()
    {
        fadeScreen = FindObjectOfType<BlackScreen>();
        cam = Camera.main.GetComponent<CameraControl>();
        story = GetComponent<StoryProgress>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PhysicObjectController>();
        box = FindObjectOfType<TextBox>();
        dialogue = FindObjectOfType<TextTyping>();
    }

    void Start()
    {
        if (story.StoryChapter() == 0 && story.ChapterEvent() == 0)
        {
            isActive = true;
            StartCoroutine(Cutscene());
            cutscenePart = 0;
            fadeScreen.SetAlpha(1);
            fadeScreen.SwitchMode(false, 15);
            playerController.DisableControl();
            dialogue.GetDialogueReference(cutsceneDialogue);
        }
    }

    void Update()
    {
        if (isActive)
        {
            cam.SetViewport(cameraTool, 0, 0.25f, 0);
            CutsceneParts();
        }

    }

    IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(5.8f);
        cutscenePart = 1;
        _timer = 0;
        yield return new WaitForSeconds(2.8f);
        cutscenePart = 2;
        _timer = 0;
        yield return new WaitForSeconds(1.5f);
        cutscenePart = 3;
        _timer = 0;
        yield return new WaitForSeconds(4f);
        cutscenePart = 4;
        _timer = 0;
    }

    void CutsceneParts()
    {
        if (_timer != -1)
        {
            _timer++;
        }

        switch (cutscenePart)
        {
            case 0:
                playerController.parentPivot = bed.transform;
                playerController.currentAction = 27;
                cameraTool.position = new Vector3(-1.95f, 0.5f, -5.5f);

                if(_timer > 80)
                {
                    _timer = -1;
                    box.BoxState(true);
                }

                break;

            case 1:

                if (cameraTool.position.x < 3)
                {
                    cameraTool.position += new Vector3(0.05f, 0, 0);
                }
                else
                {
                    cameraTool.position = new Vector3(3f, 0.5f, -5.5f);
                }
                break;

            case 2:
                if (cameraTool.position.x > -1.95f)
                {
                    cameraTool.position += new Vector3(-0.05f, 0, 0);
                }
                else
                {
                    cameraTool.position = new Vector3(-1.95f, 0.5f, -5.5f);
                }
                break;

            case 3:
                _timer++;
                playerController.parentPivot = null;
                playerController.currentAction = 28;
                cameraTool.position = new Vector3(-1.95f, 0.5f, -5.5f);

                if (_timer > 80)
                {
                    _timer = -1;
                }

                break;

            case 4:

                if (isActive)
                {
                    playerController.RestoreControl(0);
                    player.transform.rotation = Quaternion.Euler(0, 0, 0);
                    cam.SetViewport(player.transform, -7, 0.5f, 5);
                    playerController.currentAction = 0;
                    isActive = false;
                    box.BoxState(false);
                }

                break;
        }
    }
}
