using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    PhysicObjectController objectController;
    Transform player;

    [SerializeField] private float angle;
    [SerializeField] private float speed;
    [SerializeField] private int action;
    [SerializeField] private float height;
    private float distanceToPlayer;
    [SerializeField] private float noticeDist;
    [SerializeField] private float wonderTime;
    private float waitAttack;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        objectController = GetComponent<PhysicObjectController>();
    }

    void LateUpdate()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        height = transform.position.y - objectController.solidLevel;

        if (action == 0)
        {
            objectController.Move(speed, angle, 0, false);

            if (wonderTime <= 0)
            {
                speed = Random.Range(2, 10);
                angle = Random.Range(0, 360);
                wonderTime = Random.Range(120, 320);
            }
            else
            {
                Wondering();
            }

            if(objectController._knockback > 0 && transform.position.y > objectController.solidLevel + 0.5f)
            {
                objectController.transform.position -= new Vector3(0, 0.05f, 0);
            }
        }

        if (action == 1)
        {
            objectController.Move(speed, angle, 1, false);
            Notice();
        }

        if(action == 2)
        {
            objectController.Move(speed, angle, 2, false);
            Attack();
        }
        if(action == 3)
        {
            objectController.Move(speed, angle, 0, false);
            Grounded();
        }
    }

    void Wondering()
    {
        wonderTime--;
        speed -= 0.05f;
        objectController.currentAction = 1;

        if(height < 3 && objectController._knockback <= 0)
        {
            objectController.transform.position += new Vector3(0, 0.05f, 0);
        }

        if (distanceToPlayer < noticeDist && height >= 3)
        {
            transform.position = new Vector3(transform.position.x, 3, transform.position.z);
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            angle = transform.eulerAngles.y;

            waitAttack = Random.Range(8, 16);
            action = 1;
        }
    }

    void Notice()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        angle = transform.eulerAngles.y;
        waitAttack -= 0.5f;

        if(waitAttack <= 0)
        {
            action = 2;
        }
    }

    void Attack()
    {
        speed = 25;

        if(transform.position.y > objectController.solidLevel + 0.5f)
        {
            if(distanceToPlayer < 8)
            {
                objectController.isAttacking = true;
                objectController.currentAction = 2;
                objectController.transform.position -= new Vector3(0, 0.05f, 0);
            }
            else
            {
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            }
        }
        else
        {
            speed = 0;
            action = 3;
            waitAttack = Random.Range(20, 45);
        }
    }

    void Grounded()
    {
        objectController.currentAction = 0;
        objectController.isAttacking = false;

        waitAttack -= 0.05f;
        speed = 0;
        transform.position = new Vector3(transform.position.x, objectController.solidLevel + 0.1f, transform.position.z);

        if(waitAttack <= 0 || objectController._knockback > 0)
        {
            action = 0;
        }
    }
}
