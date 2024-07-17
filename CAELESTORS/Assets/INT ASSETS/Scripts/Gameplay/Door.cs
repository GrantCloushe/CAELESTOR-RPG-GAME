using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator doorAnim;
    BoxCollider doorCollider;
    JumpToSceneTrigger jump;

    [SerializeField] private bool hasOpened;
    [SerializeField] private bool activateJump;
    [SerializeField] private bool isLocked;
    [SerializeField] private int keyLevel;
    private float _delay;
    void Start()
    {
        doorCollider = GetComponent<BoxCollider>();
        doorAnim = GetComponentInChildren<Animator>();
        jump = GetComponent<JumpToSceneTrigger>();
    }

    void Update()
    {
        doorCollider.enabled = !hasOpened;

        if(doorAnim != null)
        {
            doorAnim.SetBool("Open", hasOpened);
            doorAnim.SetBool("Locked", isLocked);
        }

        if (hasOpened)
        {
            _delay++;

            if(_delay > 45 && activateJump)
            {
                jump.Activate();
            }
        }
    }

    public void Interaction(bool _jump = true)
    {
        if (!isLocked)
        {
            doorAnim.SetBool("Closing", false);
            activateJump = _jump;
            hasOpened = true;
        }
    }

    public void Interaction(int key, bool _jump = true)
    {
        if (isLocked)
        {
            if(key == keyLevel)
            {
                isLocked = false;
            }
        }
        else
        {
            activateJump = _jump;
            hasOpened = true;
        }
    }

    public void CloseDoor()
    {
        doorAnim.SetBool("Closing", true);
    }
}
