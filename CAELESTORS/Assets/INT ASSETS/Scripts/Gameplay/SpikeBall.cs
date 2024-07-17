using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    Transform ball;
    [SerializeField] private float stateSwitchTime;
    [SerializeField] private float upForce;
    [SerializeField] private float gravityForce;
    [SerializeField] private float topHeight;
    [SerializeField] private float currentHeight;
    private bool state;

    void Start()
    {
        ball = GetComponent<Transform>();
        InvokeRepeating("SwitchState", stateSwitchTime, stateSwitchTime);
    }

    void Update()
    {
        ball.position = new Vector3(ball.position.x, currentHeight, ball.position.z);

        if (state)
        {
            currentHeight += upForce;

            if(currentHeight > topHeight)
            {
                currentHeight = topHeight;
            }
        }
        else
        {
            currentHeight -= gravityForce;

            if(currentHeight < 0)
            {
                currentHeight = 0;
            }
        }
    }

    void SwitchState()
    {
        state = !state;
    }
}
