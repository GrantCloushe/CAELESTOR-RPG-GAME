using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
    [SerializeField] private int aw = 8;
    [SerializeField] private int ah = 7;
    [SerializeField] private int frames = 30;
    [SerializeField] private bool showFps;

    private void Start()
    {
        Application.targetFrameRate = frames;
        SetRatio(aw, ah);
    }

    private void Update()
    {
        if (showFps)
        {
            Debug.Log(Application.targetFrameRate);
        }
    }
    void SetRatio(float w, float h)
    {
        if ((((float)Screen.width) / ((float)Screen.height)) > w / h)
        {
            Screen.SetResolution((int)(((float)Screen.height) * (w / h)), Screen.height, true);
        }
        else
        {
            Screen.SetResolution(Screen.width, (int)(((float)Screen.width) * (h / w)), true);
        }
    }

}
