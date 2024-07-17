using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPapperUI : MonoBehaviour
{
    [SerializeField] private bool showPapper;
    [SerializeField] private bool showTask;
    [SerializeField] private float distance;
    [SerializeField] private float posY;
    [SerializeField] private float scale;
    [SerializeField] private RectTransform general;
    [SerializeField] private RectTransform leftEdge;
    [SerializeField] private RectTransform rightEdge;
    [SerializeField] private RectTransform task;
    [SerializeField] private Image taskImage;
    [SerializeField] private float taskAlpha;

    void Start()
    {
        posY = -252;
    }

    
    void Update()
    {
        leftEdge.anchoredPosition = new Vector2((distance * -1), general.anchoredPosition.y);
        rightEdge.anchoredPosition = new Vector2((distance), general.anchoredPosition.y);
        task.anchoredPosition = new Vector2(general.anchoredPosition.x, general.anchoredPosition.y);
        scale = distance / 35;
        general.localScale = new Vector2(scale * 0.7f, 1f);
        general.anchoredPosition = new Vector2(0, posY);
        taskImage.color = new Color(1, 1, 1, taskAlpha);

        if (showPapper)
        {
            ShowPapper();
        }
        else
        {
            HidePapper();
        }

        if (showTask)
        {
            ShowTask();
        }
        else
        {
            if(taskAlpha > 0)
            {
                taskAlpha -= 0.025f;
            }
        }
    }

    void ShowPapper()
    {
        if (posY < -62)
        {
            distance = 10;
            posY += 4;
        }
        else
        {
            posY = -62;
        }

        if (posY == -62)
        {
            if (distance < 64 && !showTask)
            {
                distance++;
            }
            else
            {
                if (!showTask)
                {
                    Invoke("HideTask", 2.5f);
                    showTask = true;
                }
            }
        }
    }

    void HidePapper()
    {
        if (posY > -256)
        {
            posY -= 6;
        }
        else
        {
            posY = -256;
        }
    }

    void ShowTask()
    {
        if(taskAlpha < 1)
        {
            taskAlpha += 0.025f;
        }
    }
    
    void HideTask()
    {
        showTask = false;
        showPapper = false;
        distance--;
    }

    public void TaskAppear()
    {
        showPapper = true;
        showTask = false;
        distance = 10;
        taskAlpha = 0;
    }

    public void TaskImage(Sprite img)
    {
        taskImage.sprite = img;
    }
}
