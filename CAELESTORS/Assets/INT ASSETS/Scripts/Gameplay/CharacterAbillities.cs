using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbillities : MonoBehaviour
{
    CharacterManager charManager;
    PhysicObjectController character;
    GameObject[] Targets;
    GameObject[] Enemies;
    public Transform CurrentTarget;

    [SerializeField] private bool SwordJump;
    [SerializeField] private int maxJumpsRecovery;
    private int jumps;

    [SerializeField] private bool HomingAttack;
    [SerializeField] private float HomingDistance;
    float HomingRecovery;

    [SerializeField] private bool swordHoming;
    [SerializeField] private float swordHomingDistance;
    [SerializeField] private int swordHomingCounts;
    float swordHomingDelay;
    [SerializeField] int swordHomingsAttack;
    bool hitTarget;

    [SerializeField] private bool Glide;
    private float glideCurrentTime;

    [SerializeField] private bool Helicopter;
    private float helicopterTime;

    [SerializeField] private bool Stomp;

    [SerializeField] private bool Dash;
    [SerializeField] private bool dashCharged;
    [SerializeField] private float dashChargeTime;
    [SerializeField] private float dashTime;
    private float dashCharge;
    private float dashingTime;

    [SerializeField] private bool SkySlide;
    [SerializeField] private GameObject SkySlideSurface;
    private float skySkideRecovery;

    [SerializeField] private bool KillAura;
    [SerializeField] private float killAuraActivation;
    private float killAuraTimer;

    private void Start()
    {
        character = GetComponent<PhysicObjectController>();
        charManager = GetComponentInChildren<CharacterManager>();
    }

    void Update()
    {
        if (SwordJump)
        {
            SwordJumpAction();
        }

        if (HomingAttack)
        {
            HomingAction();
        }

        if (swordHoming)
        {
            SwordHomingAction();
        }

        if (Glide)
        {
            GlidingAction();
        }

        if (Stomp)
        {
            StompAction();
        }

        if (Dash)
        {
            DashAction();
        }

        if (KillAura)
        {
            KillAuraAction();
        }
    }

    void SwordJumpAction()
    {
        if (character.isJumping && character.currentAction == 2 && jumps < maxJumpsRecovery)
        {
            character.currentAction = 22;
            jumps++;
            character.Jump(2);
        }

        if (character.isGrounded)
        {
            jumps = 0;
        }
    }

    void SwordHomingAction()
    {
       if(character.isGrounded && Input.GetButton("Fire2") && character.currentAction == 0)
        {
            swordHomingDelay++;

            if(swordHomingDelay > 35)
            {
                character.currentAction = 23;
                swordHomingsAttack = swordHomingCounts;
            }

        }
        if(character.isGrounded && Input.GetButton("Fire2") && character.currentAction == 23)
        {
            swordHomingDelay++;
        }

        if(swordHomingDelay > 60 && !Input.GetButton("Fire2"))
        {
            character.currentAction = 24;
        }

        if(swordHomingDelay > 60)
        {
            charManager.ArcanaSkin(true);
        }

       if(character.currentAction == 24 && CurrentTarget == null && swordHomingsAttack > 0)
        {
            swordHomingsAttack--;
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Homing(Enemies, swordHomingDistance);
        }
       if(character.currentAction == 24 && CurrentTarget == null && swordHomingsAttack == 0)
        {
            swordHomingDelay = 0;
            CurrentTarget = null;
            hitTarget = false;
            character.currentAction = 0;
            charManager.ArcanaSkin(false);
        }

       if(CurrentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(CurrentTarget.position, transform.position);

            if (distanceToTarget > 2 && !hitTarget)
            {
                transform.LookAt(new Vector3(CurrentTarget.position.x, transform.position.y, CurrentTarget.position.z));

                if (CurrentTarget.position.y > transform.position.y)
                {
                    transform.position += new Vector3(0, 0.05f, 0);
                }
                if (CurrentTarget.position.y > transform.position.y)
                {
                    transform.position += new Vector3(0, 0.05f, 0);
                }
            }

            if(distanceToTarget <= 2)
            {
                hitTarget = true;
            }

            if(distanceToTarget > swordHomingDistance)
            {
                hitTarget = false;
                CurrentTarget = null;
            }

            if(hitTarget && distanceToTarget > 5)
            {
                if(swordHomingsAttack > 0)
                {
                    hitTarget = false;
                    CurrentTarget = null;
                }
                else
                {
                    swordHomingDelay = 0;
                    CurrentTarget = null;
                    hitTarget = false;
                    character.currentAction = 0;
                    charManager.ArcanaSkin(false);
                }
            }
        }

    }
    void HomingAction()
    {
        if (character.isJumping && character.currentAction == 1 && HomingRecovery <= 0)
        {
            Targets = GameObject.FindGameObjectsWithTag("Target");
            character.currentAction = 17;
            HomingRecovery = 200;
            Homing(Targets, HomingDistance);
        }

        if (!character.isJumping || character.currentAction != 17)
        {
            CurrentTarget = null;
        }

        if (CurrentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(CurrentTarget.position, transform.position);

            if (distanceToTarget > 2)
            {
                transform.LookAt(new Vector3(CurrentTarget.position.x, transform.position.y, CurrentTarget.position.z));

                if (CurrentTarget.position.y + 1.5f > transform.position.y)
                {
                    transform.position += new Vector3(0, 0.25f, 0);
                }
            }
            else
            {
                character.currentAction = 3;
                character.Jump(2);
                HomingRecovery = 0;
                CurrentTarget = null;
            }
        }

        if(HomingRecovery > 0)
        {
            HomingRecovery--;
        }
        else
        {
            HomingRecovery = 0;
        }
    }
    void Homing(GameObject[] targets, float targetingDistance)
    {
        float minDist = targetingDistance;

        foreach(GameObject t in targets)
        {
            float distance = Vector3.Distance(t.transform.position, transform.position);

            if(distance < minDist)
            {
                CurrentTarget = t.transform;
                minDist = distance;
            }
        }
    }

    void GlidingAction()
    {
        if(character.isJumping && Input.GetButtonDown("Fire3"))
        {
            character.currentAction = 19;
        }

        if(character.currentAction == 19)
        {
            if (!Input.GetButton("Fire3"))
            {
                character.currentAction = 3;
                character.disableGravity = false;
            }
            if (transform.position.y < character.solidLevel + 0.5f)
            {
                character.currentAction = 20;
            }
        }
    }

    void StompAction()
    {
        if(character.isJumping && character.currentAction == 2)
        {
            character.currentAction = 18;
        }
    }

    void DashAction()
    {
        if (character.isGrounded && Input.GetButton("Fire2") && character.currentAction == 0 && !dashCharged)
        {
            dashCharge++;

            if (dashCharge > 8)
            {
                character.currentAction = 23;
            }

        }
        if (character.isGrounded && Input.GetButton("Fire2") && character.currentAction == 23)
        {
            dashCharge++;
        }

        if (dashCharge > dashChargeTime)
        {
            character.currentAction = 0;
            dashCharged = true;
            charManager.ArcanaSkin(true);
        }

        if(dashCharged && !Input.GetButton("Fire2"))
        {
            dashCharge = 0;
            character.currentAction = 25;
            charManager.ArcanaSkin(false);
        }

        if(character.currentAction == 25 && dashCharged)
        {
            dashingTime++;

            if(dashingTime > dashTime)
            {
                dashCharged = false;
                character.currentAction = 0;
            }
        }
        else
        {
            if(character.currentAction != 25)
            {
                dashingTime = 0;
            }
        }
    }

    void KillAuraAction()
    {
        if(character.isGrounded && Input.GetButton("Fire1") && character.currentAction == 0)
        {
            killAuraActivation++;

            if(killAuraActivation > 10)
            {
                killAuraActivation = 0;
                character.currentAction = 21;
            }
            
        }

        if(character.currentAction == 21)
        {
            killAuraTimer++;
            if (!Input.GetButton("Fire1"))
            {
                character.currentAction = 0;
            }
        }
    }

    public void SwitchAbillity_SwordJump(bool enable)
    {
        SwordJump = enable;
    }
    public void SwitchAbillity_Homing(bool enable)
    {
        HomingAttack = enable;
    }
    public void SwitchAbillity_SwordHoming(bool enable)
    {
        swordHoming = enable;
    }
    public void SwitchAbillity_Glide(bool enable)
    {
        Glide = enable;
    }
    public void SwitchAbillity_Stomp(bool enable)
    {
        Stomp = enable;
    }
    public void SwitchAbillity_Dash(bool enable)
    {
        Dash = enable;
    }
}
