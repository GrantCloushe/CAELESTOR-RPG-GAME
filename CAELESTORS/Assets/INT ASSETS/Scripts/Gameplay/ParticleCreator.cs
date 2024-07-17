using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCreator : MonoBehaviour
{
    PhysicObjectController obj;
    [SerializeField] private GameObject arcanaSFX;
    [SerializeField] private GameObject resourceBlinkSfx;
    bool creatingArcanaParticles;
    bool jump;

    void Start()
    {
        obj = GetComponentInParent<PhysicObjectController>();
    }

    void Update()
    {
        if (obj.currentAction == 23 && !creatingArcanaParticles)
        {
            creatingArcanaParticles = true;
            InvokeRepeating("CreateArcana", 0.25f, 0.25f);
        }
        if (obj.currentAction == 0)
        {
            CancelInvoke("CreateArcana");
            creatingArcanaParticles = false;
        }
    }

    void CreateArcana()
    {
        float rX = Random.Range(-5f, 5f);
        float rY = Random.Range(-5f, 5f);
        float rZ = Random.Range(-5f, 5f);
        Vector3 randomVel = new Vector3(rX, rY, rZ);
        GameObject ArcanaParticle = Instantiate(arcanaSFX, transform);
        ParticlePhysic physic = ArcanaParticle.GetComponent<ParticlePhysic>();
        physic.SetVelocity(randomVel);
        physic.TargetSetup(transform, 4, 0.15f, false);
        physic.Initiated();
    }

    public void CreateBlink(Transform pos)
    {
        GameObject BlinkSfx = Instantiate(resourceBlinkSfx);
        BlinkSfx.transform.position = pos.position - new Vector3(0, 0.5f, 0);

    }
}
