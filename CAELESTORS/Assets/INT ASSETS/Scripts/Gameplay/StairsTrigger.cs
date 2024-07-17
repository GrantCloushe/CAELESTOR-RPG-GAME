using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    FloorSystem floorEngine;

    public bool locked;
    private bool changeFloor;
    public Transform elevatingPivot;

    [SerializeField] private bool isElevator;
    [SerializeField] private float elevatingSpeed;
    [SerializeField] private float setDelay;
    [SerializeField] private float elevatingDelay;
    [SerializeField] private int goToFloor;
    [SerializeField] private Vector3 elevatingDirection;
    [SerializeField] private Transform cagePivot;
    [SerializeField] private float cageLenght;
    Vector3 resetPosition;
    bool reset;

    private void Awake()
    {
        floorEngine = FindObjectOfType<FloorSystem>();

        if (!reset)
        {
            resetPosition = transform.position;
            reset = true;
        }
    }

    private void Update()
    {
        if (changeFloor && elevatingDelay <= 0)
        {
            elevatingDelay = 0;
            floorEngine.Floor(goToFloor);
            changeFloor = false;
        }

        if (elevatingDelay > 0 && isElevator)
        {
            elevatingDelay--;

            float dirX = elevatingDirection.x;
            float dirY = elevatingDirection.y;
            float dirZ = elevatingDirection.z;
            Vector3 direction = new Vector3(elevatingSpeed * dirX, elevatingSpeed * dirY, elevatingSpeed * dirZ);
            transform.position += direction;
        }

        if(cagePivot != null)
        {
            Cage();
        }
    }

    public void ChangeFloor()
    {
        if (!changeFloor && !locked)
        {
            elevatingDelay = setDelay;
            changeFloor = true;
        }
    }

    public bool IsElevating()
    {
        if (elevatingDelay > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void Cage()
    {
        cagePivot.localScale = new Vector3(cageLenght, 1, 1);

        if(elevatingDelay <= 0)
        {
            if(cageLenght > 0.1)
            {
                cageLenght -= 0.05f;
            }
            else
            {
                cageLenght = 0.1f;
            }
        }
        else
        {
            if (cageLenght < 1)
            {
                cageLenght += 0.05f;
            }
            else
            {
                cageLenght = 1;
            }
        }
    }

    private void OnEnable()
    {
        transform.position = resetPosition;
    }
}
