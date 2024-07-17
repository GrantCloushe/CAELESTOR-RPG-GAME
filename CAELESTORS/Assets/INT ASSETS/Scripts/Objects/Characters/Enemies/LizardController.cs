using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardController : MonoBehaviour
{
    PhysicObjectController objectController;
    Transform player;
    GameObject[] navPoint;

    [SerializeField] private float angle;
    [SerializeField] private float speed;
    [SerializeField] private int action;
    private float distanceToPlayer;
    [SerializeField] private float noticeDist;
    private float wonderTime;
    private float waitAttack;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        objectController = GetComponent<PhysicObjectController>();
        navPoint = GameObject.FindGameObjectsWithTag("NavPoint");
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(action == 0)
        {
            objectController.Move(speed, angle, 0, false);

            if (wonderTime <= 0)
            {
                angle = Random.Range(0, 360);
                wonderTime = Random.Range(120, 320);
            }
            else
            {
                Wondering();
            }
        }

        if(action == 1)
        {
            objectController.Move(speed, angle, 0, false);
            Notice();
        }

        if(action == 2 && objectController._knockback < 1)
        {
            Attack();
        }

        if(action == 3 && objectController._knockback < 1)
        {
            CoolDown();
        }

        if (!objectController.isGrounded)
        {
            objectController.isAttacking = true;
        }
        else
        {
            objectController.isAttacking = false;
        }

        if(objectController.solidLevel > 1000)
        {
            objectController.Move(speed, angle, 0, true);
            speed = 10;
            transform.LookAt(new Vector3(navPoint[0].transform.position.x, transform.position.y, navPoint[0].transform.position.x));
            angle = transform.eulerAngles.y;
        }

        if(objectController._knockback > 1)
        {
            objectController.isAttacking = false;
        }
    }

    void Wondering()
    {
        wonderTime--;
        speed = 4;

        if(distanceToPlayer < noticeDist)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            angle = transform.eulerAngles.y;

            waitAttack = Random.Range(3, 6);
            action = 1;
        }

        if (!objectController.isGrounded)
        {
            angle = angle + 45;
        }
    }

    void Notice()
    {
        if (waitAttack > 0)
        {
            waitAttack -= 0.25f;
            speed = 0;
        }
        else
        {
            speed = 8;

            if(distanceToPlayer > noticeDist)
            {
                waitAttack = 15;
                action = 3;
            }

            if(distanceToPlayer < 5)
            {
                action = 2;
            }
        }
    }

    void Attack()
    {
        speed = 12;

        if (objectController.isGrounded)
        {
            objectController.Move(speed, angle, 0, true);
        }
        else
        {
            if (objectController.isJumping)
            {
                objectController.Move(speed, angle, 0, false);
            }
        }

        objectController.isAttacking = true;

        if (objectController.isGrounded)
        {
            waitAttack = 15;
            action = 3;
            objectController.isAttacking = false;
        }
    }

    void CoolDown()
    {
        objectController.Move(speed, angle, 0, false);

        if (objectController.isGrounded)
        {
            if (waitAttack > 0)
            {
                speed = 0;
                waitAttack -= 0.05f;
            }
            else
            {
                action = 0;
            }

            if (distanceToPlayer < noticeDist && waitAttack == 0)
            {
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                angle = transform.eulerAngles.y;

                waitAttack = Random.Range(3, 6);
                action = 1;
            }
        }
        else
        {
            speed = 12;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Stopper" && objectController.isGrounded)
        {
            wonderTime += 15;
            angle = angle + (180 + Random.Range(-45, 45));
        }
    }
}
