using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class TextTyping : MonoBehaviour
{
    [Header("TextBox")]
    [SerializeField] TextBox box;
    [SerializeField] Image cutsceneImage;
    [SerializeField] Image speakerImage;
    [SerializeField] Animator cutsceneAnimator;
    [SerializeField] private int animationState;
    [SerializeField] private bool pictureShow;
    private float imageAlpha;

    [Header("TextSettings")]
    [SerializeField] private Text speakerName;
    [SerializeField] private Text dialogueLine;
    [SerializeField] private string currentTextLine;
    [SerializeField] private int currentLine;
    [SerializeField] private int endLine;
    [SerializeField] private float textSpeed;
    [SerializeField] private float lineSkipSpeed;
    [SerializeField] private bool autoDialogue;
    private bool newLine;

    [Header("DialogueEvents")]
    [SerializeField] DialogueReference Dialogue;
    public static Action<int> EventAction;

    private void Start()
    {
        speakerImage.enabled = false;

        if (cutsceneImage != null)
        {
            cutsceneImage.enabled = true;
        }

        imageAlpha = 0;
    }

    void Update()
    {
        if (newLine)
        {
            if(Dialogue != null)
            {
                NextLine();
                newLine = false;
            }
        }

        if(Dialogue != null)
        {
            textSpeed = Dialogue.lines[currentLine].textSpeed;
            autoDialogue = Dialogue.lines[currentLine].autoSkip;

            if(Dialogue.lines[currentLine].speaker != null)
            {
                speakerName.text = Dialogue.lines[currentLine].speaker.SpeakerName;
                speakerName.color = Dialogue.lines[currentLine].speaker.nameColor;
            }
            else
            {
                speakerName.text = null;
            }

            if(cutsceneImage != null)
            {
                CutscenePicture();
            }

            if (Dialogue.lines[currentLine].cameraControlSettings.setCamera)
            {
                CameraControl(Dialogue.lines[currentLine].cameraControlSettings.toolCamera);
            }
        }
    }

    public void GetDialogueReference(DialogueReference reference)
    {
        Dialogue = reference;
    }

    public void ResetDialogueLine()
    {
        Dialogue = null;
        currentLine = 0;
    }

    public void NewLine()
    {
        string text = null;
        dialogueLine.text = null;

        bool hideBoxSate = Dialogue.lines[currentLine].hideBox;
        box.BoxState(!hideBoxSate, true);

        if (Dialogue != null)
        {
            currentLine = Dialogue.startLine;
            endLine = Dialogue.endLine;
            currentTextLine = Dialogue.lines[currentLine].textLine;

            CameraPosition();
            Speaker();
            Lines();

            EventAction?.Invoke(Dialogue.lines[currentLine].setEvent);
        }

        if(text != currentTextLine)
        {
            text = currentTextLine;
            SetAnimationState();

            if (!autoDialogue)
            {
                lineSkipSpeed = Dialogue.lines[currentLine].skipTime;
            }
            else
            {
                lineSkipSpeed = currentTextLine.Length / (1.25f / textSpeed);
            }

            StartCoroutine(TypeLine(text, lineSkipSpeed));
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        dialogueLine.text = null;
        box.BoxState(false);
        speakerImage.enabled = false;
    }

    void NextLine()
    {
        string text = null;
        dialogueLine.text = null;
        StopCoroutine(TypeLine(text, lineSkipSpeed));

        if (currentLine < endLine - 1)
        {
            currentLine++;

            if (text != currentTextLine)
            {
                currentTextLine = Dialogue.lines[currentLine].textLine;
                text = currentTextLine;

                SetAnimationState();
                CameraPosition();
                Speaker();
                Lines();

                EventAction?.Invoke(Dialogue.lines[currentLine].setEvent);

                bool hideBoxSate = Dialogue.lines[currentLine].hideBox;
                box.BoxState(!hideBoxSate, true);

                if (!autoDialogue)
                {
                    lineSkipSpeed = Dialogue.lines[currentLine].skipTime;
                }
                else
                {
                    lineSkipSpeed = currentTextLine.Length / (1.25f / textSpeed);
                }

                StartCoroutine(TypeLine(text, lineSkipSpeed));
            }
        }
        else
        {
            EndDialogue();
        }

    } 

    IEnumerator TypeLine(string textLine, float skip)
    {
        foreach (char c in textLine.ToCharArray())
        {
            dialogueLine.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(skip);
        newLine = true;
    }

    void SetAnimationState()
    {
        if(cutsceneAnimator != null)
        {
            animationState = Dialogue.lines[currentLine].animState;
            cutsceneAnimator.SetInteger("animState", animationState);
        }
    }

    void CutscenePicture()
    {
        pictureShow = Dialogue.lines[currentLine].pictureShow;
        animationState = Dialogue.lines[currentLine].animState;
        cutsceneImage.color = new Color(1, 1, 1, imageAlpha);

        if (pictureShow && imageAlpha < 1.2f)
        {
            imageAlpha = imageAlpha + 0.05f;
        }
        else
        {
            if (imageAlpha > 0)
            {
                imageAlpha = imageAlpha - 0.05f;
            }
        }
    }

    void CameraPosition()
    {
        if(Dialogue.lines[currentLine].cameraControlSettings.setPosition != null)
        {
            Transform Camera = Dialogue.lines[currentLine].cameraControlSettings.toolCamera;
            Transform Position = Dialogue.lines[currentLine].cameraControlSettings.setPosition;
            Camera.position = Position.position;
        }
        else
        {
            return;
        }
    }

    void CameraControl(Transform toolCamera)
    {
        Vector3 movementDirection = Dialogue.lines[currentLine].cameraControlSettings.movementDirection;
        float movementSpeed = Dialogue.lines[currentLine].cameraControlSettings.movementSpeed;
        float rotation = Dialogue.lines[currentLine].cameraControlSettings.rotation;
        float pitch = Dialogue.lines[currentLine].cameraControlSettings.pitch;
        float rotationSpeed = Dialogue.lines[currentLine].cameraControlSettings.rotateSpeed;

        toolCamera.position += movementDirection * movementSpeed;

        if (Dialogue.lines[currentLine].cameraControlSettings.fixedRotation)
        {
            toolCamera.rotation = Quaternion.Euler(pitch, rotation, 0);
        }
        else
        {
            float rotate = toolCamera.rotation.eulerAngles.y;
            toolCamera.rotation = Quaternion.Euler(pitch, rotate + rotationSpeed, 0);
        }
    }

    void Speaker()
    {

        if (Dialogue.lines[currentLine].speaker != null)
        {
            speakerImage.enabled = true;
            Speaker currentSpeaker = Dialogue.lines[currentLine].speaker;
            int faceSet = Dialogue.lines[currentLine].speakerEmotion;
            speakerImage.sprite = currentSpeaker.FaceSet[faceSet];
        }
        else
        {
            speakerImage.enabled = false;
        }
    }

    void Lines()
    {
        if (Dialogue.lines[currentLine].setLineSettings.setNewLines)
        {
            Dialogue.startLine = Dialogue.lines[currentLine].setLineSettings.newStartLine;
            Dialogue.endLine = Dialogue.lines[currentLine].setLineSettings.newEndLine;
        }
        else
        {
            return;
        }
    }

    public DialogueReference currentDialogue()
    {
        return Dialogue;
    }
}
