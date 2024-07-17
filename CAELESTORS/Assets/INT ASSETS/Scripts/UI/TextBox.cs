using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    Animator anim;
    Image box;
    [SerializeField] private bool activeState;
    [SerializeField] private GameObject textField;
    [SerializeField] private GameObject speaker;
    [SerializeField] private TextTyping dialogue;
    [SerializeField] private int _state;
    private bool _handleMode;

    private void Start()
    {
        anim = GetComponent<Animator>();
        box = GetComponent<Image>();
        box.enabled = false;
    }

    void Update()
    {
        anim.SetBool("ActiveState", activeState);
    }

    public void SetState(int state)
    {
        _state = state;

        if(state < 2)
        {
            box.enabled = true;
        }

        if(state == 1)
        {
            if (!_handleMode)
            {
                dialogue.NewLine();
            }
            textField.SetActive(true);
            speaker.SetActive(true);
        }
        else
        {
            textField.SetActive(false);
            speaker.SetActive(false);
        }
    }

    public void BoxState(bool animState, bool handle = false)
    {
        activeState = animState;
        _handleMode = handle;
    }

    public int BoxAnimationState()
    {
        return _state;
    }
    public bool BoxActiveState()
    {
        return activeState;
    }
}
