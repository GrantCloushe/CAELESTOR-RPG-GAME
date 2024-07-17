using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatonController : MonoBehaviour
{
    PhysicObjectController objectController;
    Transform player;

    [SerializeField] private float angle;
    [SerializeField] private float speed;
    [SerializeField] private float clockwork;
    [SerializeField] private float clockSwitch;
    [SerializeField] private bool clockStart;
    [SerializeField] private int action;
    private float distanceToPlayer;
    [SerializeField] private float noticeDist;

    bool resetClockwork;
    int side;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        objectController = GetComponent<PhysicObjectController>();
        InvokeRepeating("SideSwitch", clockSwitch, clockSwitch);
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (clockStart && clockwork > 0 && !objectController.isStunning)
        {
            AutomatonStart();
        }
        else
        {
            objectController.Move(0, transform.eulerAngles.y, 0, false);

            if (!resetClockwork)
            {
                resetClockwork = true;
                Invoke("ResetClock", 3);
            }
        }
    }

    void ResetClock()
    {
        clockwork = 250;
        resetClockwork = false;
    }

    void AutomatonStart()
    {
        clockwork -= 0.25f;
        objectController.Move(speed, angle, action, false);

        switch (side)
        {
            case 1:
                angle += 0.25f;
                break;
            case 3:
                angle -= 0.25f;
                break;
        }

        if (action == 0 && objectController.currentAction == 0 && distanceToPlayer < noticeDist && distanceToPlayer > 2)
        {
            speed = 3;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            angle = transform.eulerAngles.y;
        }

        if(distanceToPlayer < 1.5f)
        {
            speed = 0;

            if(action == 0 && !objectController.isStunning && objectController.isGrounded)
            {
                action = 2;
                Invoke("Attack", Random.Range(0.45f, 0.8f));
            }
        }
        else
        {
            if(action != 0)
            {
                action = 0;
            }
        }
    }

    void Attack()
    {
        Invoke("Recovery", Random.Range(0.25f, 0.75f));
    }

    void Recovery()
    {
        objectController.currentAction = 0;
    }

    void SideSwitch()
    {
        side = Random.Range(1, 3);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Stopper" && objectController.isGrounded)
        {
            angle = angle + (180 + Random.Range(-45, 45));
        }
    }
}
