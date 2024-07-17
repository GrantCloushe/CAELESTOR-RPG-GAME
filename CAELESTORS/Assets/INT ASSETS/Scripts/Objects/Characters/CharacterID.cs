using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterID : MonoBehaviour
{
    public static int characterID = 0;
    private CharacterManager character;

    private void Awake()
    {
        character = FindObjectOfType<CharacterManager>();
        if(character != null)
        {
            character.SetCharacter(characterID);
        }
    }

    public void SetCharacter(int character)
    {
        characterID = character;
    }
}
