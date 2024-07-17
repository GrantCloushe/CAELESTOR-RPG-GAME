using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mode7Projection : MonoBehaviour
{
    RectTransform projectionTransform;
    RawImage projection;
    [SerializeField] private float heightScale = 1;
    [SerializeField] private float heightPosition = 0f;

    void Start()
    {
        projectionTransform = GetComponent<RectTransform>();
        projection = GetComponent<RawImage>();
    }


    void Update()
    {
        projectionTransform.localScale = new Vector2(1, heightScale);
    }

    public void HeightScale(float pitch)
    {
        heightScale = (pitch / 12) + 1;

        if(heightScale > 1)
        {
            projection.uvRect = new Rect(0, 0, 1, 1);
        }
        else
        {
            projection.uvRect = new Rect(0, 0, 1, 0.5f);
        }
    }
}
