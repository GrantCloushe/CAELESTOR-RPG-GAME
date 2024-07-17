using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerostatsFlight : MonoBehaviour
{
    [SerializeField] private float yPos;
    [SerializeField] private float localScale;

    void Start()
    {
        TextTyping.EventAction += Action;
        yPos = -13;
        localScale = 1;

    }

    private void Update()
    {
        float amp = Mathf.Sin(0.25f * Time.time) * 0.1f;
        transform.position = new Vector3(transform.position.x, yPos + Mathf.Sin(2.5f * Time.time) * 0.25f, transform.position.z);
        transform.localScale = new Vector3(localScale, localScale, localScale);
    }

    void Action(int action)
    {
        if(action == 1 && yPos < -10.5f)
        {
            yPos += 0.05f;
        }

        if(action == 2)
        {
            yPos += 0.05f;
            localScale = localScale - 0.015f;
        }

        if(localScale < 0)
        {
            TextTyping.EventAction -= Action;
            Destroy(gameObject);
        }
    }
}
