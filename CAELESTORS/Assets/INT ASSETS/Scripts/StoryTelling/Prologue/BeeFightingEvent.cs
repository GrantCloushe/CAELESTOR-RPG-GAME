using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeFightingEvent : MonoBehaviour
{
    DialogueReference dialog;
    GameObject player;

    TextBox box;
    TextTyping textDialog;
    [SerializeField] private GameObject beeObject;
    [SerializeField] private EventTrigger eventTrig;
    [SerializeField] private int sceneState;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dialog = GetComponent<DialogueReference>();
        box = FindObjectOfType<TextBox>();
        textDialog = FindObjectOfType<TextTyping>();
        beeObject.SetActive(false);
    }

    void Update()
    {
        if (eventTrig.Activation() && sceneState == 0)
        {
            textDialog.GetDialogueReference(dialog);
            box.BoxState(true);
            TextTyping.EventAction += Action;
            sceneState = 1;
            EventStart();
        }

        if(sceneState == 1)
        {
            Event();
        }
    }
    void EventStart()
    {
        if(sceneState == 1)
        {
            beeObject.GetComponent<BeeController>().enabled = false;
            beeObject.GetComponent<CharacterController>().enabled = false;
            beeObject.GetComponent<PhysicObjectController>().currentAction = 1;

            beeObject.SetActive(true);
            beeObject.transform.position = player.transform.position + (player.transform.forward * 10);
        }
    }

    void Event()
    {
        beeObject.GetComponent<PhysicObjectController>().currentAction = 1;

        if (beeObject.transform.position.y < 2)
        {
            beeObject.transform.position += new Vector3(0, 0.005f, 0);
        }
    }

    void Action(int action)
    {
        if(action == 1)
        {
            sceneState = 2;
            beeObject.GetComponent<BeeController>().enabled = true;
            beeObject.GetComponent<CharacterController>().enabled = true;
        }
    }

}
