using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSystem : MonoBehaviour
{
    BlackScreen blackScreen;
    PlayerInteraction player;

    [SerializeField] private GameObject[] floors;
    [SerializeField] private int floor;
    [SerializeField] private bool changeFloor;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        blackScreen = FindObjectOfType<BlackScreen>();
    }

    void Start()
    {
        ChangeFloorToNew();
    }


    void Update()
    {
        if (changeFloor)
        {
            if(blackScreen != null)
            {
                blackScreen.SwitchMode(true, 25, true);

                if(blackScreen.State() == true)
                {
                    ChangeFloorToNew();
                    changeFloor = false;
                }
            }
        }
    }

    void ChangeFloorToNew()
    {
        floors[floor].SetActive(true);
        player.Spawn(floor);

        for (int i = 0; i < floors.Length; i++)
        {
            if(i != floor)
            {
                floors[i].SetActive(false);
            }
        }
    }

    public void Floor(int _floor)
    {
        floor = _floor;
        changeFloor = true;
    }

}
