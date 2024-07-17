using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameOverResult : MonoBehaviour
{
    Image img;
    GameObject player;
    [SerializeField] private float alpha;
    [SerializeField] private float textScale;
    [SerializeField] private float textAlpha;
    [SerializeField] private RectTransform text;
    [SerializeField] private Image textImage;
    [SerializeField] private Sprite[] resultSprite;

    public static Action ResultEnd;
    public static Action SceneEnd;

    bool showResult;
    bool showVictory;
    bool sceneEnd;
    int timing;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        img = GetComponent<Image>();
    }

    void Update()
    {
        img.color = new Color(1, 1, 1, alpha);
        text.localScale = new Vector2(textScale/2, textScale/2);
        textImage.color = new Color(1, 1, 1, textAlpha);

        if (showResult)
        {
            timing++;

            if (alpha < 1)
            {
                alpha += 0.05f;
            }
            if (textScale < 1)
            {
                textScale += 0.025f;
            }

            if(timing > 360)
            {
                textAlpha -= 0.05f;
            }

            if(timing > 480)
            {
                showResult = false;
                timing = 0;
                player.GetComponent<PlayerInteraction>().Spawn();
            }
        }
        else
        {
            if(alpha > 0 && !showVictory && !sceneEnd)
            {
                alpha -= 0.05f;
            }
        }
    }

    public void ShowResult(int resultType = 0)
    {
        textImage.sprite = resultSprite[resultType];
        timing = 0;
        showResult = true;
        alpha = 0;
        textScale = 0;
        textAlpha = 1;

    }

    public void ShowVictory()
    {
        showVictory = true;
        textImage.sprite = resultSprite[2];
        timing++;
        textAlpha = 1;

        if (textScale < 1 && timing < 420)
        {
            textScale += 0.025f;
        }

        if(timing > 420)
        {
            textAlpha -= 0.05f;

            if (alpha < 1)
            {
                alpha += 0.05f;
            }
            if(timing > 480)
            {
                SceneManager.LoadScene(0);
                ResultEnd.Invoke();
            }
        }
    }

    public void ChangeScene()
    {
        sceneEnd = true;
        if(alpha < 1)
        {
            alpha += 0.025f;
        }
        else
        {
            SceneEnd.Invoke();
        }
    }
}
