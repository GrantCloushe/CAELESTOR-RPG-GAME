using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    PhysicObjectController characterController;
    PlayerInterface playerRes;
    [SerializeField] private int character;
    [SerializeField] private GameObject[] Characters;
    [SerializeField] private List<Attributes> CharacterAttributes;
    [SerializeField] private bool ParsekianHoming;

    int charIndex;
    int i;

    private void Awake()
    {
        
    }

    private void Start()
    {
        characterController = GetComponentInParent<PhysicObjectController>();
        ChangeSkin();
    }

    void Update()
    {
        if(charIndex != character)
        {
            ChangeSkin();
            charIndex = character;
        }
    }

    void ChangeSkin()
    {
        SwitchCharacterAbillity();
        Characters[character].SetActive(true);

        float moveSpd = CharacterAttributes[character].moveSpeed;
        float acc = CharacterAttributes[character].acceleration;
        float decc = CharacterAttributes[character].deceleration;
        int jumpCount = CharacterAttributes[character].jumpsCount;
        float jumpForce = CharacterAttributes[character].jumpForce;
        float gravityForce = CharacterAttributes[character].gravityForce;
        float str = CharacterAttributes[character].str;
        float def = CharacterAttributes[character].def;
        float knockStr = CharacterAttributes[character].knockStr;
        float deftime = CharacterAttributes[character].defTime;

        int _str = CharacterAttributes[character].mod_str;
        int _dex = CharacterAttributes[character].mod_dex;
        int _spd = CharacterAttributes[character].mod_spd;
        int _def = CharacterAttributes[character].mod_def;

        characterController.SetAttributes(moveSpd, acc, decc, jumpCount, jumpForce, gravityForce);
        characterController.SetStats(str, def, knockStr, deftime);
        characterController.Modifiers(_str, _dex, _spd, _def);

        for (i = 0; i < Characters.Length; i++)
        {
            if (i != character)
            {
                Characters[i].SetActive(false);
            }
        }

    }
    public void ArcanaSkin(bool state)
    {
        CharacterAttributes[character].ArcanaSkin.SetActive(state);
    }

    [System.Serializable]
    public struct Attributes
    {
        public string CharacterName;
        public GameObject ArcanaSkin;

        [Header("MovementValues")]
        public float moveSpeed;
        public float acceleration;
        public float deceleration;
        public int jumpsCount;
        public float jumpForce;
        public float gravityForce;

        [Header("CombatValues")]
        public float str;
        public float def;
        public float knockStr;
        public float defTime;

        [Header("Modifiers")]
        public int mod_str;
        public int mod_dex;
        public int mod_spd;
        public int mod_def;

        [Header("Resourses")]
        public int res_bronze;
        public int res_ferrum;
        public int res_gold;
        public int res_crystal;
        public int res_arrows;
    }

    void SwitchCharacterAbillity()
    {
        CharacterAbillities abillity = GetComponentInParent<CharacterAbillities>();

        switch (character)
        {
            case 0:
                abillity.SwitchAbillity_Homing(true);
                abillity.SwitchAbillity_Dash(true);
                abillity.SwitchAbillity_Stomp(true);
                abillity.SwitchAbillity_Glide(false);
                abillity.SwitchAbillity_SwordHoming(false);
                abillity.SwitchAbillity_SwordJump(false);
                break;
            case 1:
                abillity.SwitchAbillity_Homing(false);
                abillity.SwitchAbillity_Dash(false);
                abillity.SwitchAbillity_Stomp(true);
                abillity.SwitchAbillity_Glide(false);
                abillity.SwitchAbillity_SwordHoming(true);
                abillity.SwitchAbillity_SwordJump(true);
                break;
            case 2:
                abillity.SwitchAbillity_Homing(false);
                abillity.SwitchAbillity_Dash(false);
                abillity.SwitchAbillity_Stomp(true);
                abillity.SwitchAbillity_Glide(true);
                abillity.SwitchAbillity_SwordHoming(true);
                abillity.SwitchAbillity_SwordJump(false);
                break;
        }
    }

    public int Character()
    {
        return character;
    }

    public void CharacterVisible(bool enable = true)
    {
        Characters[character].SetActive(enable);
    }

    public void SetCharacter(int id)
    {
        character = id;
    }
}
