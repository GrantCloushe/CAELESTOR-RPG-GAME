using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SelectionBase]
public class PhysicObjectController : MonoBehaviour
{

    CharacterController controller;
    Transform Object;

    [Header("MovementValues")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    private float curSpeed;
    public float currentSpeed;

    [Header("GravityValues")]
    [SerializeField] private float gravityForce;
    public float solidLevel;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float topHeight = 2.5f;
    public bool disableGravity;
    public bool isFlying;
    public bool isGrounded;
    [SerializeField] private int multipleJump;
    private int currentJumps;
    float kayotTime = 0;
    public Transform parentPivot;
    public Vector3 setPosition;
    public bool setPos;

    [Header("ActionValues")]
    [SerializeField] private float damageTaken;
    [SerializeField] private float damageDealt;
    [SerializeField] private float damageAbsorbing;
    [SerializeField] private float knockbackPower;
    [SerializeField] private float guardTime;
    private float guard;
    [HideInInspector] public float _knockback;
    [HideInInspector] public float _damageTaken;

    [Header("ActionStatements")]
    public int currentAction;
    [SerializeField] private float guardRecovery;
    public bool isCarrying;
    public bool isAttacking;
    public bool isGuarding;
    public bool isStunning;
    public bool isDodging;

    [Header("PlayerControl")]
    [SerializeField] private bool isPlayable;
    [SerializeField] private int playerMode;
    [SerializeField] private int inputsOrientation = 1;
    public bool isJumping;

    [Header("Modifiers")]
    [SerializeField, Range(1, 10)] private int str;
    [SerializeField, Range(1, 10)] private int dex;
    [SerializeField, Range(1, 10)] private int spd;
    [SerializeField, Range(1, 10)] private int def;

    LayerMask collisionMask;
    shadowProjection shadow;
    HurtBox hurtbox;
    HoldingItem item;

    void Awake()
    {
        Object = GetComponent<Transform>();
        collisionMask = LayerMask.GetMask("Ground");
        controller = GetComponent<CharacterController>();
        Vector3 forward = transform.forward;
        shadow = GetComponentInChildren<shadowProjection>();
        hurtbox = GetComponentInChildren<HurtBox>();
        item = GetComponent<HoldingItem>();

        if (isPlayable)
        {
            inputsOrientation = 1;
        }
    }

    void Update()
    {
        _knockback = knockbackPower;
        _damageTaken = damageTaken;

        Movement();

        if (isPlayable)
        {
            float hor = Input.GetAxis("Horizontal") * inputsOrientation;
            float ver = Input.GetAxis("Vertical") * inputsOrientation;
            bool Fire1 = Input.GetButtonDown("Fire1");
            bool Fire2 = Input.GetButtonDown("Fire2");

            switch (playerMode)
            {
                case 1:
                    if (currentAction == 0)
                    {
                        FirstPersonMovement(hor, ver, Fire1, Fire2);
                    }
                    break;
            }
        }

        if (shadow != null)
        {
            shadow.castShadow(Object, solidLevel);
        }

        if (hurtbox != null)
        {
            hurtbox.isActive = isAttacking;

            if (hurtbox.hasBlocked)
            {
                currentAction = 16;
                hurtbox.hasBlocked = false;
            }

            if (item != null)
            {
                HurtboxParametres((damageDealt + item.weaponStr) * 0.75f, item.weaponKnockback * 0.5f, item.weaponVericality);
            }
            else
            {
                HurtboxParametres(damageDealt * 0.25f, 1.5f, 0.05f);
            }
        }

    }

    private void LateUpdate()
    {
        if (transform.position.y > topHeight)
        {
            Vector3 topHeightPos = new Vector3(transform.position.x, topHeight, transform.position.z);
            transform.position = topHeightPos;
        }

        if (parentPivot != null)
        {
            transform.position = parentPivot.position;
        }

        if (setPos && currentAction != 0)
        {
            transform.position = setPosition;
        }

        if (transform.position.y < -5 && !isFlying)
        {
            damageTaken = 0;
            curSpeed = 0;

            if (!isPlayable)
            {
                Destroy(gameObject);
            }

            if (isPlayable && GetComponent<CharacterController>().enabled)
            {
                gameObject.GetComponent<CharacterController>().enabled = false;
                GetComponent<PlayerInteraction>().ShowResult(0);
            }
        }
    }

    public void SetAttributes(float moveSpd, float acc, float decc, int jumps, float jmpForce, float gravity)
    {
        moveSpeed = moveSpd;
        acceleration = acc;
        decceleration = decc;
        multipleJump = jumps - 1;
        jumpForce = jmpForce;
        gravityForce = gravity;
    }

    public void SetStats(float dmg, float def, float knockbackPow, float defTime)
    {
        damageDealt = dmg;
        damageAbsorbing = def;
        knockbackPower = knockbackPow;
        guardTime = defTime;
    }

    public void Modifiers(int _str, int _dex, int _spd, int _def)
    {
        str = _str;
        dex = _dex;
        spd = _spd;
        def = _def;
    }

    void Movement()
    {
        currentSpeed = curSpeed;
        Vector3 move = transform.forward;
        controller.Move(move * (curSpeed * (GetModifier(2) / 10) / 100));

        if (!disableGravity)
        {
            GroundLevel();
            Gravity();
        }
        if (currentAction == 3 || currentAction == 0)
        {
            if (disableGravity && !isFlying) { disableGravity = false; }
        }

        if (isAttacking && currentAction != 6)
        {
            if(curSpeed > 0)
            {
                curSpeed -= decceleration / 15;
            }
            else
            {
                curSpeed = 0;
            }
        }

        if (isGuarding)
        {
            Guard();
        }

        if (isGrounded)
        {
            currentJumps = multipleJump;
        }

        if (isCarrying)
        {
            currentAction = 14;
        }

        if (knockbackPower > 0 || currentAction == 5 || currentAction == 7)
        {
            HitStun();
        }

        switch (currentAction)
        {
            case 7:
                KnockFall();
                break;
            case 6:
                Slide(currentSpeed);
                break;
            case 8:
                Grab();
                break;
            case 9:
                Guard();
                break;
            case 10:
                Trampoline();
                break;
            case 11:
                WindRing();
                break;
            case 12:
                Pole();
                break;
            case 13:
                Pulley();
                break;
            case 14:
                HoldingBlock();
                break;
            case 15:
                CatapultLaunch();
                break;
            case 16:
                Crushed();
                break;
            case 17:
                Homing();
                break;
            case 18:
                Stomp();
                break;
            case 19:
                Glide();
                break;
            case 20:
                GlideSliding();
                break;
            case 21:
                KillAura();
                break;
            case 22:
                SwordJump();
                break;
            case 23:
                SwordHomingCharge();
                break;
            case 24:
                SwordCharge();
                break;
            case 25:
                Dashing();
                break;
            case 26:
                VictoryIdle();
                break;
            case 27:
                Lying();
                break;
            case 28:
                LyingGetUp();
                break;
            case 29:
                Sitting();
                break;
        }
    }

    void GroundLevel()
    {
        float currentPosition = transform.position.y;

        RaycastHit ray;

        if (Physics.Raycast(transform.position, Vector3.down, out ray, 1000f, collisionMask))
        {
            float currentSolidLevel = ray.point.y;
            solidLevel = Mathf.Round(currentSolidLevel * 100.0f) * 0.01f;
        }
        else
        {
            solidLevel = -9999f;
        }

        if (currentPosition - solidLevel < 0.59f)
        {
            isGrounded = true;
            kayotTime = currentSpeed;
        }
        else if (currentPosition - solidLevel > 0.59f && kayotTime <= 0)
        {
            isGrounded = false;
        }
        else if (currentPosition - solidLevel > 0.59f && kayotTime > 0)
        {
            kayotTime -= 1.5f;
        }
    }

    void Gravity()
    {
        velocity.y += gravityForce / 10 * Time.deltaTime;
        controller.Move(velocity);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        if (isGrounded && velocity.y <= 0 && isJumping && currentAction != 8 && currentAction != 20)
        {
            currentAction = 0;
            isJumping = false;
        }

        if (velocity.y > 0)
        {
            isJumping = true;

            if (curSpeed > 2)
            {
                curSpeed -= decceleration * 0.012f;
            }
            else if (curSpeed < -1)
            {
                curSpeed += decceleration * 0.012f;
            }
        }

        if (velocity.y != 0 && currentAction != 0)
        {
            if (isPlayable && currentAction != 8 && knockbackPower < 1)
            {
                float hor = Input.GetAxis("Horizontal") * inputsOrientation;
                float ver = Input.GetAxis("Vertical") * inputsOrientation;
                AirControl(hor, ver);
            }
        }
    }

    public void RestoreControl(int restoreMode)
    {
        if (restoreMode == 0)
        {
            restoreMode = 1;
        }

        isPlayable = true;
        inputsOrientation = 1;
        playerMode = restoreMode;

        return;
    }

    void HurtboxParametres(float attackStr, float knockbackPow, float verticalImp)
    {
        switch (currentAction)
        {
            case 1:
                hurtbox.damageDealt = attackStr * (GetModifier(0) / 10) / 2;
                hurtbox.knockback = knockbackPow * (GetModifier(0) / 10) / 2;
                hurtbox.verticalImpulse = verticalImp * (GetModifier(0) / 10) / 2;
                hurtbox.CrushingSTR = str + 4;
                break;
            case 2:
                hurtbox.damageDealt = attackStr * (GetModifier(0) / 10);
                hurtbox.knockback = knockbackPow * (GetModifier(0) / 10);
                hurtbox.verticalImpulse = verticalImp * (GetModifier(0) / 10);
                hurtbox.CrushingSTR = str - 1;
                break;
            case 5:
                hurtbox.damageDealt = attackStr * (GetModifier(0) / 10) / 3;
                hurtbox.knockback = knockbackPow * (GetModifier(0) / 10) / 2;
                hurtbox.verticalImpulse = verticalImp * 2 * (GetModifier(0) / 10);
                break;
            case 17:
                hurtbox.damageDealt = attackStr * (GetModifier(0) / 10) / 5;
                hurtbox.knockback = knockbackPow * (GetModifier(0) / 10) / 5;
                hurtbox.verticalImpulse = verticalImp * (GetModifier(0) / 10) * 0.25f;
                break;
            case 18:
                hurtbox.damageDealt = attackStr * (GetModifier(0) / 10);
                hurtbox.knockback = knockbackPow * (GetModifier(0) / 10) / 2;
                hurtbox.verticalImpulse = verticalImp * (GetModifier(0) / 10) * 1.25f;
                break;

        }
    }

    public void Move(float speed, float angle, int action, bool jumpAction)
    {
        curSpeed = speed;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        if (!jumpAction)
        {
            currentAction = action;
        }

        if (action == 0 && jumpAction)
        {
            currentAction = 3;
            Jump(curSpeed);
        }
    }

    public void InputsActionControl(int inputButton)
    {
        currentAction = inputButton;
    }

    public void DisableControl()
    {
        curSpeed = 0;
        isPlayable = false;
        inputsOrientation = 0;

        return;
    }

    void FirstPersonMovement(float horInput, float verInput, bool fire1, bool fire2)
    {
        isAttacking = false;

        curSpeed = Mathf.Clamp(curSpeed, -moveSpeed / 4, moveSpeed);
        curSpeed += (acceleration / 10 * verInput);

        if (curSpeed > 0 && verInput == 0)
        {
            curSpeed -= decceleration / 10;

            if (curSpeed <= 0.5f)
            {
                curSpeed = 0;
            }
        }

        if (curSpeed < 0 && verInput == 0)
        {
            curSpeed += decceleration / 10;

            if (curSpeed >= -0.5f)
            {
                curSpeed = 0;
            }
        }

        transform.Rotate(0, horInput * turnSpeed, 0);
        transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            currentJumps -= 1;
            Jump(curSpeed);
            currentAction = 3;
        }

        if (Input.GetButtonDown("Fire3") && isGrounded && curSpeed > 2)
        {
            curSpeed += 15;
            isAttacking = true;
            isDodging = true;
            hurtbox.isActive = true;
            currentAction = 6;
        }

        if (Input.GetButtonDown("Fire3") && isGrounded && curSpeed < 2 && guardRecovery == 0)
        {
            isGuarding = true;
            currentAction = 9;
        }

        if (fire1 || fire2)
        {
            if (fire1)
            {
                InputsActionControl(1);
            }

            if (fire2)
            {
                InputsActionControl(2);
            }
        }

        if (guardRecovery > 0)
        {
            guardRecovery--;
        }
        else
        {
            guardRecovery = 0;
        }
    }

    public void Jump(float strength)
    {
        float curJumpForce = jumpForce / ((multipleJump + 1) - currentJumps) + 1; ;

        velocity.y = Mathf.Sqrt((curJumpForce + curSpeed * 0.012f) / 30 * -2f * gravityForce / 10);

        if (curSpeed > 2)
        {
            curSpeed -= decceleration * (acceleration / decceleration) * 0.02f;
        }
    }

    void Slide(float speed)
    {
        float ver = Input.GetAxis("Vertical") * inputsOrientation;
        curSpeed -= decceleration * 0.15f;
        curSpeed += (ver * acceleration) / 7;

        if (curSpeed < 0)
        {
            curSpeed = 0;
            isAttacking = false;
            hurtbox.isActive = false;
            isDodging = false;
            currentAction = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump(curSpeed);
            isAttacking = false;
            hurtbox.isActive = false;
            isDodging = false;
            currentAction = 3;
        }
    }

    void AirControl(float hor, float ver)
    {
        curSpeed += ((acceleration * 0.012f) * ver);

        transform.Rotate(0, hor * turnSpeed / 2, 0);

        if (Input.GetButtonDown("Jump") && currentJumps > 0)
        {
            Jump(curSpeed + 5);
            currentJumps -= 1;
            currentAction = 3;
        }

        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InputsActionControl(1);
            }

            if (Input.GetButtonDown("Fire2"))
            {
                InputsActionControl(2);
            }
        }
    }

    public void GetHurt(float damage, float knockback, Transform target, float verticalImpulse)
    {
        damageTaken += damage / (damageAbsorbing * (GetModifier(3) * 0.1f));
        guardRecovery += damage / (damageAbsorbing * (GetModifier(3) * 0.8f));
        knockbackPower += (damageTaken * knockback) / (damageAbsorbing * 1.75f * (GetModifier(3) * 0.1f));
        velocity.y = Mathf.Sqrt((verticalImpulse + knockbackPower * 0.012f) / 30 * -2f * gravityForce / 10);

        if (currentAction != 5)
        {
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            currentAction = 5;
            curSpeed = 0;
        }
    }

    void HitStun()
    {
        curSpeed = 0;
        knockbackPower -= 0.5f;
        knockbackPower = Mathf.Clamp(knockbackPower, 0, 450);
        Vector3 knockbackDirection = -transform.forward;
        controller.Move(knockbackDirection * (knockbackPower / 100));

        if (knockbackPower < 1)
        {
            isStunning = false;
            currentAction = 0;
        }
        else
        {
            isStunning = true;
        }

        if (currentAction == 5 && isGrounded == false)
        {
            Invoke("KnockFall", 0.75f);
        }

    }

    void KnockFall()
    {
        if (knockbackPower < 1)
        {
            currentAction = 3;
        }

        if (knockbackPower > 1 && !isGrounded)
        {
            currentAction = 7;
        }

        if (isGrounded)
        {
            currentAction = 5;
        }
    }

    void Grab()
    {
        isGrounded = false;
        currentJumps = multipleJump;

        if (Input.GetButtonDown("Jump"))
        {
            parentPivot = null;
            Jump(curSpeed);
            currentAction = 3;
        }
    }

    void Guard()
    {
        guard++;

        if (guard >= guardTime)
        {
            isGuarding = false;
            guardRecovery = 30;
            guard = 0;
            currentAction = 0;
        }
    }

    void Trampoline()
    {

    }

    void WindRing()
    {
        curSpeed = curSpeed - (decceleration / 25);
        currentJumps = multipleJump;
        velocity.y = 0;

        if (curSpeed < 0)
        {
            currentAction = 3;
        }
    }

    void Pole()
    {
        currentJumps = multipleJump;
        float position = transform.position.y - 0.05f;
        disableGravity = true;
        setPosition = new Vector3(transform.position.x, position, transform.position.z);

        if (isPlayable)
        {
            if (Input.GetButtonDown("Jump"))
            {
                setPos = false;
                disableGravity = false;
                Jump(curSpeed);
                currentAction = 3;
                curSpeed += 20;
            }
        }

        if (position - solidLevel < 0.6f)
        {
            setPos = false;
            disableGravity = false;
            currentAction = 0;
        }
    }

    void Pulley()
    {
        disableGravity = true;
        if (isPlayable)
        {
            if (Input.GetButtonDown("Jump"))
            {
                disableGravity = false;
                parentPivot = null;
                Jump(curSpeed);
                currentAction = 3;
            }
        }
    }

    void HoldingBlock()
    {
        if (isPlayable)
        {
            float hor = Input.GetAxis("Horizontal") * inputsOrientation;
            float ver = Input.GetAxis("Vertical") * inputsOrientation;
            bool Fire1 = Input.GetButtonDown("Fire1");
            bool Fire2 = Input.GetButtonDown("Fire2");

            curSpeed = Mathf.Clamp(curSpeed, 0, moveSpeed / 4);

            if (ver > 0.1f)
            {
                curSpeed += (acceleration / 10 * ver);
            }

            if (curSpeed > 0 && ver == 0)
            {
                curSpeed -= decceleration / 10;

                if (curSpeed <= 0.5f)
                {
                    curSpeed = 0;
                }
            }

            transform.Rotate(0, hor * turnSpeed, 0);
            transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump(curSpeed / 4);
            }
        }

        if (!isCarrying)
        {
            currentAction = 0;
        }
    }

    void CatapultLaunch()
    {
        isGrounded = false;

        if (curSpeed < 5)
        {
            currentAction = 3;
        }
    }

    void Crushed()
    {
        guardRecovery = 200;
        GetHurt(3, 2, transform, 0);
    }

    void Homing()
    {
        isAttacking = true;
        currentJumps = multipleJump;
        curSpeed = moveSpeed;
    }

    void Stomp()
    {
        isAttacking = true;
        velocity.y -= 0.25f;
    }

    void Glide()
    {
        isAttacking = true;

        if (isPlayable)
        {
            float hor = Input.GetAxis("Horizontal") * inputsOrientation;
            transform.Rotate(0, hor * turnSpeed, 0);
        }

        disableGravity = true;
        Vector3 glide = new Vector3(0, -0.01f, 0);
        controller.Move(glide);
        curSpeed = moveSpeed;

        if (knockbackPower > 0)
        {
            disableGravity = false;
            currentAction = 5;
        }
    }

    void GlideSliding()
    {
        disableGravity = false;
        curSpeed -= decceleration / 225;

        if (curSpeed < 2)
        {
            currentAction = 0;
            isAttacking = false;
        }
        else
        {
            isAttacking = true;
        }
    }

    void KillAura()
    {
        if (isPlayable)
        {
            float hor = Input.GetAxis("Horizontal") * inputsOrientation;
            transform.Rotate(0, hor * turnSpeed, 0);
        }

        curSpeed = 5;
        isAttacking = true;
    }

    void SwordJump()
    {
        curSpeed += 0.5f;
    }

    void SwordHomingCharge()
    {
        curSpeed = 0;

        if (isPlayable)
        {
            if (!Input.GetButton("Fire2"))
            {
                currentAction = 0;
            }
        }
    }

    void SwordCharge()
    {
        isAttacking = true;
        curSpeed = moveSpeed * 4;
    }

    void Dashing()
    {
        isAttacking = true;

        curSpeed = moveSpeed * 2;

        if (isPlayable)
        {
            float hor = Input.GetAxis("Horizontal") * inputsOrientation;
            transform.Rotate(0, hor * turnSpeed, 0);

            if (Input.GetButtonDown("Jump"))
            {
                disableGravity = false;
                parentPivot = null;
                Jump(curSpeed);
                currentAction = 3;
            }
        }
    }

    void VictoryIdle()
    {
        if (isGrounded)
        {
            curSpeed = 0;
            controller.enabled = false;
        }
    }

    void Lying()
    {
        curSpeed = 0;
        controller.enabled = false;
    }

    void LyingGetUp()
    {
        curSpeed = 0;
        controller.enabled = true;
    }

    void Sitting()
    {
        transform.rotation = parentPivot.rotation;
    }

    void Elevating()
    {
        isCarrying = false;
        disableGravity = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit col)
    {
        if (col.gameObject.CompareTag("Obstacle"))
        {
            if(knockbackPower > 0)
            {
                GetHurt(2, 0.1f, col.transform, 0.12f);
            }
        }

        if (col.gameObject.CompareTag("Character") || col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Enemy"))
        {
            Physics.IgnoreCollision(controller, col.gameObject.GetComponent<CharacterController>());
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "GrabRope")
        {
            if (currentAction == 3)
            {
                col.GetComponent<GrabRope>().Grab(currentSpeed);
                parentPivot = col.transform;
                currentAction = 8;
            }

        }

        if (col.tag == "Trampoline")
        {
            col.GetComponent<Trampoline>().Active();
            parentPivot = col.transform;
        }

        if (col.tag == "WindRing")
        {
            if (currentAction == 3)
            {
                curSpeed += 20;
                currentAction = 11;
            }
        }

        if (col.tag == "Pole")
        {
            if (currentAction == 3)
            {
                setPos = true;
                setPosition = new Vector3(col.transform.position.x, transform.position.y, col.transform.position.z);
                curSpeed = 0;
                currentAction = 12;
            }
        }

        if (col.tag == "BuildingArea")
        {
            col.GetComponent<BuildingArea>().ShowText();
        }

        if (col.tag == "Pulley")
        {
            if (currentAction == 3)
            {
                currentAction = 13;
                parentPivot = col.transform;
                transform.rotation = col.transform.rotation;
                col.GetComponentInParent<Pulley>().active = true;
            }
        }

        if(col.tag == "Resource")
        {
            if (isPlayable)
            {
                CharacterInventory inv = GetComponent<CharacterInventory>();
                ParticleCreator particle = GetComponentInChildren<ParticleCreator>();
                if (particle != null)
                {
                    particle.CreateBlink(col.transform);
                }
                int ind = col.GetComponent<ResourcePhysics>().Index();
                int count = col.GetComponent<ResourcePhysics>().Count();

                if (inv != null)
                {
                    inv.AddResource(ind, count);
                }
                Destroy(col.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.tag == "Hurtbox")
        {
            HurtBox hurtbox = col.GetComponent<HurtBox>();

            if (hurtbox.isActive && controller != hurtbox.ignoreCollider && guardRecovery==0)
            {
                int randomDefence = UnityEngine.Random.Range(1, 10) + def;
                int randomAttack = UnityEngine.Random.Range(1, 10) + hurtbox.CrushingSTR;

                if (randomDefence > randomAttack)
                {
                    isGuarding = true;
                    currentAction = 9;
                }
                else
                {
                    currentAction = 16;
                }
            }

            if (hurtbox.isActive && controller != hurtbox.ignoreCollider && !isGuarding && !isDodging)
            {
                GetHurt(hurtbox.damageDealt, hurtbox.knockback, col.transform, hurtbox.verticalImpulse);
            }

            if(hurtbox.isActive && controller != hurtbox.ignoreCollider && isGuarding)
            {
                hurtbox.hasBlocked = true;
            }
        }

        if(col.tag == "Hazard")
        {
            if (col.GetComponent<Hazard>().active && !isDodging)
            {
                if(col.GetComponent<Hazard>().isProjectile() == false)
                {
                    float dmg = col.GetComponent<Hazard>().damage;
                    float knock = col.GetComponent<Hazard>().knockback;
                    GetHurt(2.25f + dmg, 5.5f + knock, col.transform, 0.02f);
                }

                if (col.GetComponent<Hazard>().isProjectile())
                {
                    if (!isGuarding && !isAttacking)
                    {
                        float dmg = col.GetComponent<Hazard>().damage;
                        float knock = col.GetComponent<Hazard>().knockback;
                        GetHurt(2.25f + dmg, 5.5f + knock, col.transform, 0.02f);
                    }
                    else
                    {
                        if (isGuarding)
                        {
                            Destroy(col.gameObject);
                        }
                        if (isAttacking)
                        {
                            col.transform.rotation = transform.rotation;
                        }
                    }
                }
            }
        }

        if(col.tag == "Propellor")
        {
            velocity = col.GetComponent<Propellor>().propellorForce;
        }

        if (col.tag == "Trampoline")
        {
            Trampoline tramp = col.GetComponent<Trampoline>();
            bool trampState = tramp.JumpState();

            if(trampState == false)
            {
                currentAction = 10;
            }
            else
            {
              Jump(((velocity.y) * 50) + 10f);
              parentPivot = null;
            }
        }

        if(col.tag == "Pulley")
        {
            bool active = col.GetComponentInParent<Pulley>().active;

            if(active == false)
            {
                disableGravity = false;
                parentPivot = null;
                currentAction = 0;
            }
        }

        if (col.tag == "CatapultLaunchZone")
        {
            Catapult cat = col.GetComponentInParent<Catapult>();

            if (Input.GetButtonDown("Fire1") && !isCarrying && isPlayable)
            {
                cat.control = !cat.control;
            }
            if (Input.GetButtonDown("Fire2") && isPlayable)
            {
                cat.control = false;
                cat.launch = true;
            }

            if (cat.control && isPlayable)
            {
                float hor = Input.GetAxis("Horizontal") * inputsOrientation;
                parentPivot = col.transform;
                transform.rotation = col.transform.rotation;
                cat.Rotate(hor * 3);

                if (isCarrying)
                {
                    cat.control = false;
                }
            }
            else if(parentPivot = col.transform)
            {
                if (!cat.control)
                {
                    parentPivot = null;
                }
            }

            if(cat.launchTimer > 40 && cat.launchTimer < 55 && !cat.laucnhBlock)
            {
                transform.rotation = col.transform.rotation;
                Jump(25);
                curSpeed += 35;
                currentAction = 15;
            }


        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.tag == "Pole")
        {
            if(currentAction == 12)
            {
                setPos = false;
                disableGravity = false;
                currentAction = 0;
            }
        }

        if(col.tag == "CatapultLaunchZone")
        {
            Catapult cat = col.GetComponentInParent<Catapult>();
            cat.control = false;
        }

        if (col.tag == "BuildingArea")
        {
            col.GetComponent<BuildingArea>().HideText();
        }
    }

    public float GetModifier(int ind)
    {
        switch (ind)
        {
            case 0:
                return str;
            case 1:
                return dex;
            case 2:
                return spd;
            case 3:
                return def;
            default:
                return 0;
        }
    }

    public bool IsPlayable()
    {
        return isPlayable;
    }

}
