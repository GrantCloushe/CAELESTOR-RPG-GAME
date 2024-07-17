using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    CharacterID charId;
    [SerializeField] private int menuState;
    [SerializeField] private float headerScale;
    [SerializeField] private RectTransform header;
    [SerializeField] private Image pressStart;
    [SerializeField] private GameObject characterSelect;
    [SerializeField] private int currentCharacter;
    [SerializeField] private GameObject[] characters;
    [SerializeField] private RawImage blackScreen;
    [SerializeField] private float alpha;
    private bool pressBlink;
    
    void Start()
    {
        alpha = 0;
        pressStart.enabled = false;
        characterSelect.SetActive(false);
        charId = GetComponent<CharacterID>();
    }

    
    void Update()
    {
        header.localScale = new Vector2(headerScale + 1.25f, headerScale/2);
        blackScreen.color = new Color(0, 0, 0, alpha);
        switch (menuState)
        {
            case 0:
                headerScale = 5;
                menuState = 1;
                break;
            case 1:
                ShowLogo();
                break;
            case 2:
                PressStart();
                break;
            case 3:
                CharacterSelect();
                break;
            case 4:
                CharacterSelected();
                break;
        }
    }

    void ShowLogo()
    {
        pressStart.enabled = false;
        if(headerScale > 1)
        {
            headerScale -= 0.15f;
        }
        else
        {
            headerScale = 1;
            menuState = 2;
            InvokeRepeating("PressRepeating", 0.25f, 0.25f);
        }
    }

    void PressStart()
    {
        pressStart.enabled = pressBlink;

        if (Input.GetButtonDown("Fire1"))
        {
            CancelInvoke("PressRepeating");
            menuState = 3;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            menuState = 3;
        }
        if (Input.GetButtonDown("Jump"))
        {
            menuState = 3;
        }
        if (Input.GetButtonDown("Submit"))
        {
            menuState = 3;
        }


    }

    void CharacterSelect()
    {
        currentCharacter = Mathf.Clamp(currentCharacter, 0, characters.Length - 1);

        if (!characterSelect.activeInHierarchy)
        {
            SwitchCharacter();
            characterSelect.SetActive(true);
        }

        CancelInvoke("PressRepeating");
        pressStart.enabled = false;
        headerScale = 0;

        if (Input.GetKeyDown("left"))
        {
            currentCharacter--;
            SwitchCharacter();
        }
        if (Input.GetKeyDown("right"))
        {
            currentCharacter++;
            SwitchCharacter();
        }
        if(currentCharacter < 0)
        {
            currentCharacter++;
            SwitchCharacter();
        }
        if(currentCharacter > characters.Length - 1)
        {
            currentCharacter--;
            SwitchCharacter();
        }

        if (Input.GetButtonDown("Submit"))
        {
            menuState = 4;
        }

    }
    void SwitchCharacter()
    {
        for(int i = 0; i < characters.Length; i++)
        {
            SpriteRenderer spr = characters[i].GetComponent<SpriteRenderer>();

            if (i != currentCharacter)
            {
                spr.color = new Color(0, 0, 0);
            }
            else
            {
                spr.color = new Color(1, 1, 1);
            }
        }
    }

    void PressRepeating()
    {
        pressBlink = !pressBlink;
    }

    void CharacterSelected()
    {
        Animator anim = characters[currentCharacter].GetComponent<Animator>();
        anim.SetBool("Selected", true);
        charId.SetCharacter(currentCharacter);

        if (alpha < 1)
        {
            alpha += 0.025f;
        }
        else
        {
            if(currentCharacter != 2)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}
