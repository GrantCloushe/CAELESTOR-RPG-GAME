using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public bool isActive;
    public CharacterController ignoreCollider;
    public float damageDealt;
    public float knockback;
    public float verticalImpulse;
    public bool hasBlocked;
    public bool crushingBlock;
    public int CrushingSTR;

    void Update()
    {
        
    }
}
