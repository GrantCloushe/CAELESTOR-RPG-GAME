using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScrolling : MonoBehaviour
{
    public float scrollX;
    public float scrollY;
    public float scrollDelay;
    float delay;

    private void Start()
    {
      InvokeRepeating("Scrolling", scrollDelay, scrollDelay);
    }

    void Scrolling()
    {
        float offsetX = Time.time * scrollX;
        float offsetY = Time.time * scrollY;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}
