using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingbatController : MonoBehaviour
{
    Transform player;
    PhysicObjectController obj;

    [SerializeField] private float shootingDelay;
    [SerializeField] private float noticeDist;
    [SerializeField] private int action;
    [SerializeField] private GameObject projectile;
    
    float distanceToPlayer;
    public float delay;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        obj = GetComponent<PhysicObjectController>();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        transform.LookAt(player);

        if(distanceToPlayer < noticeDist && action == 0)
        {
            action = 1;
            delay = shootingDelay;
            Attack();
        }

        if(obj._knockback > 0)
        {
            action = 2;
        }

        if(action == 1)
        {
            Attack();
        }

        if(action == 2)
        {
            obj.disableGravity = false;
        }
    }

    void Attack()
    {
        delay--;

        if(delay <= 0)
        {
            Instantiate(projectile);
            projectile.transform.position = transform.position + new Vector3(0, -0.5f, 0);
            projectile.transform.rotation = transform.rotation;
            delay = shootingDelay;
        }

        if(distanceToPlayer > noticeDist)
        {
            action = 0;
        }
    }
}
