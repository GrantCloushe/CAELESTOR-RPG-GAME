using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    RawImage blackScreen;
    private float alpha;
    [SerializeField] private float alphaSpeed = 50;
    [SerializeField] private bool mode;
    [SerializeField] private bool returnMode;
    void Start()
    {
        blackScreen = GetComponent<RawImage>();

    }

   
    void Update()
    {
        blackScreen.color = new Color(0, 0, 0, alpha);

        if (mode)
        {
            if(alpha < 1)
            {
                alpha += alphaSpeed / 1000;
            }
            else
            {
                mode = returnMode;
            }
        }
        else
        {
            if(alpha > 0)
            {
                alpha -= alphaSpeed / 1000;
            }
            else
            {
                mode = returnMode;
            }
        }
    }

    public bool State()
    {
        if (alpha >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SwitchMode(bool setMode, float speed = 25, bool returnAfter = false)
    {
        mode = setMode;
        alphaSpeed = speed;

        if (returnAfter)
        {
            returnMode = !mode;
        }
        else
        {
            returnMode = mode;
        }
    }

    public void SetAlpha(float _alpha)
    {
        alpha = _alpha;
    }
}
