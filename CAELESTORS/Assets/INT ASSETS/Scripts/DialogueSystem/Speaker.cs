using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speaker")]
public class Speaker : ScriptableObject
{
    public string SpeakerName;
    public int nameIcon;
    [SerializeField] public Color nameColor = new Color(1, 1, 1, 1);
    public Sprite[] FaceSet;
}
