using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueReference : MonoBehaviour
{
    public int startLine;
    public int endLine;

    public List<DialogueLine> lines;

    [System.Serializable]
    public struct CameraControl
    {
        public bool setCamera;
        public Transform toolCamera;
        public Transform setPosition;
        public bool fixedRotation;
        public int rotation;
        public int pitch;
        public float rotateSpeed;
        public Vector3 movementDirection;
        public float movementSpeed;

    }

    [System.Serializable]
    public struct SetLines
    {
        public bool setNewLines;
        public int newStartLine;
        public int newEndLine;
    }

    [System.Serializable]
    public struct DialogueLine
    {
        public string lineTitle;
        [TextArea(1,3)]public string textLine;
        public Speaker speaker;
        public int speakerEmotion;
        public bool autoSkip;
        public float textSpeed;
        public float skipTime;
        public bool pictureShow;
        public int animState;
        public bool hideBox;
        public int setEvent;

        public SetLines setLineSettings;
        public CameraControl cameraControlSettings;
    }


}