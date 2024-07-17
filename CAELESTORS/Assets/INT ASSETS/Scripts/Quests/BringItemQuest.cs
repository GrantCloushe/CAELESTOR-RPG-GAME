using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringItemQuest : MonoBehaviour
{
    NpcStatesMachine npc;
    TextTyping dialogueListening;
    private int _currentQuestState;

    private DialogueReference dialogue;
    [SerializeField] private int questActivationAction;
    [SerializeField] private Item itemToBring;
    [SerializeField] private int requiredItemState;
    [SerializeField] private int setNewNPCState;

    public DialogueReference completedDialogue;
    public bool wrongItem;
    public int CurrentQuestState;

    private void Awake()
    {
        npc = GetComponentInParent<NpcStatesMachine>();
        dialogueListening = FindObjectOfType<TextTyping>();
        dialogue = GetComponent<DialogueReference>();
    }

    void Update()
    {
        CurrentQuestState = _currentQuestState;

        if (dialogueListening.currentDialogue() == dialogue)
        {
            TextTyping.EventAction += Action;
        }
        else
        {
            TextTyping.EventAction -= Action;
        }


    }

    void Action(int action)
    {
        if(_currentQuestState == 0)
        {
            if (action == questActivationAction)
            {
                _currentQuestState = 1;
            }
        }
    }

    public void CheckItem(Item itm, int itmState)
    {
        Debug.Log("Item");
        Debug.Log(itm);

        if (_currentQuestState == 1)
        {
            if (itm == itemToBring)
            {
                if (itmState == requiredItemState)
                {
                    Debug.Log("Верный предмет");
                    _currentQuestState = 2;
                    wrongItem = false;
                    return;
                }
            }
            else
            {
                Debug.Log("Неверный предмет");
                wrongItem = true;
                return;
            }
        }
        else
        {
            return;
        }
    }

    public void EndQuest()
    {
        _currentQuestState = 3;
        npc.SetNewState(setNewNPCState);
    }
}
