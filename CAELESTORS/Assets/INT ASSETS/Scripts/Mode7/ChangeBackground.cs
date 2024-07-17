using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBackground : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private bool disableBG;
    [SerializeField] private Sprite backgroundSprite;

    public void ChangeBG()
    {
        background.enabled = !disableBG;
        background.sprite = backgroundSprite;
    }
}
