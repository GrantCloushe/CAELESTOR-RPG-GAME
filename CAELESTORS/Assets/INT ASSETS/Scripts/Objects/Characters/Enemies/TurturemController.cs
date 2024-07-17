using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurturemController : MonoBehaviour
{
    PhysicObjectController objectController;
    Transform player;

    [SerializeField] private float angle;
    [SerializeField] private float speed;
    [SerializeField] private int action;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float noticeDistance;
    [SerializeField] private float retreatDistance;
    [SerializeField] private float distanceToPlayer;

    float wonderTime;
    float attackTime;
    bool cantRetreat;
    float throwTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        objectController = GetComponent<PhysicObjectController>();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(action == 0 && objectController._knockback < 1)
        {
            objectController.Move(speed, angle, 0, false);

            if (wonderTime <= 0)
            {
                angle = Random.Range(0, 360);
                wonderTime = Random.Range(120, 400);
                rotateSpeed = -rotateSpeed;
            }
            else
            {
                Wondering();
            }
        }

        if(action == 1)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            angle = transform.eulerAngles.y;
            objectController.Move(speed, angle, 0, false);

            if(distanceToPlayer > noticeDistance)
            {
                action = 0;
            }

            if (distanceToPlayer > retreatDistance)
            {
                speed = 0;
            }
            else
            {
                if (!cantRetreat && objectController.isGrounded)
                {
                    speed = -5;
                }
                else
                {

                    if (objectController.isGrounded)
                    {
                        action = 3;
                    }
                    else
                    {
                        angle = angle + (180 + Random.Range(-45, 45));
                        action = 3;
                    }
                }
            }

            if(attackTime > 0)
            {
                attackTime -= 0.25f;
            }
            else
            {
                attackTime = RandomTime();
                action = 2;
                Attack();
            }
        }

        if(action == 2)
        {
            Attack();
        }

        if(action == 3)
        {
            Dash();
        }

    }

    void Wondering()
    {
        wonderTime -= 0.25f;
        angle = angle + rotateSpeed;
        speed = 2;

        if(distanceToPlayer < noticeDistance)
        {
            attackTime = RandomTime();
            action = 1;
        }

        if (!objectController.isGrounded)
        {
            angle = angle + 45;
        }
    }

    void Attack()
    {
        objectController.currentAction = 2;
        throwTime++;

        if (action == 2 && objectController.isGrounded)
        {
            objectController.Move(0, angle, 0, true);
            objectController.currentAction = 0;
        }

        if(throwTime > 40)
        {
            action = 0;
            Instantiate(projectile);
            projectile.transform.position = transform.position + new Vector3(0, 2, 0);
            projectile.transform.rotation = transform.rotation;
            projectile.GetComponent<Block>().destroyOnGround = true;
            projectile.GetComponent<Block>().GetImpulse(3, 1);
            throwTime = 0;
        }
    }

    void Dash()
    {
        speed = 12;
        objectController.Move(speed, angle, 0, false);
        objectController.isAttacking = true;

        if(distanceToPlayer > retreatDistance)
        {
            action = 0;
            objectController.isAttacking = false;
        }
    }

    float RandomTime()
    {
        return Random.Range(10, 45);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Stopper" && objectController.isGrounded)
        {
            cantRetreat = true;
            wonderTime += 15;
            angle = angle + (180 + Random.Range(-45, 45));
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Stopper" && objectController.isGrounded)
        {
            cantRetreat = false;
        }
    }

}
