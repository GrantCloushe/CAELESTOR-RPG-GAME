using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeFrame : MonoBehaviour
{
    CharacterManager character;
    [SerializeField] private Image frameSprite;
    [SerializeField] private Sprite[] lifeIcons;

    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CharacterManager>();
    }


    void Update()
    {
        frameSprite.sprite = lifeIcons[character.Character()];
    }
}
