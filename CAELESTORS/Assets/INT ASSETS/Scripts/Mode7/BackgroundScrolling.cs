using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScrolling : MonoBehaviour
{
    RectTransform bgTransform;
    Transform cam;
    private float worldRotation;
    private float imageWidth;
    private float scrollingParametr;
    private float scrollingOffset;

    [SerializeField] private float pictureScale = 5f;
    [SerializeField] private int scrolling = 32;
    [SerializeField] private float xOffset;

    void Start()
    {
        cam = Camera.main.transform;
        bgTransform = GetComponent<RectTransform>();
        imageWidth = bgTransform.rect.width;
    }

    void Update()
    {
        scrollingParametr = pictureScale / imageWidth;

        bgTransform.anchoredPosition = new Vector2(scrollingOffset + xOffset, bgTransform.anchoredPosition.y);
        worldRotation = cam.eulerAngles.y;

        scrollingOffset = worldRotation * -1 / (scrolling * scrollingParametr);
    }
}
