using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    CharacterManager character;
    PhysicObjectController player;
    RotationCamera playerCamera;

    [SerializeField] private GameOverResult results;    
    [SerializeField] private AudioSource stageMusic;
    [SerializeField] private List<StageMusic> stageMusicSettings;
    [SerializeField] private AudioClip VictorySound;
    [SerializeField] private bool stageCompleted;
    [SerializeField] private int[] characterTask;
    [SerializeField] private int currentTask;

    Task_DestroyAllEnemies task01;
    Task_Build task02;
    Task_Find task03;

    private void Awake()
    {
        task01 = GetComponent<Task_DestroyAllEnemies>();
        task02 = GetComponent<Task_Build>();
        task03 = GetComponent<Task_Find>();

        task01.enabled = false;
        task02.enabled = false;
        task03.enabled = false;
    }
    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<RotationCamera>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CharacterManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PhysicObjectController>();
        currentTask = characterTask[character.Character()];
        ControlTask();
        StageMusicSet();
    }

    
    void Update()
    {
        TrackTask();

        if (stageCompleted)
        {
            playerCamera.VictoryIdle();
            player.currentAction = 26;
            results.ShowVictory();
            if(stageMusic.clip != VictorySound)
            {
                stageMusic.Stop();
                stageMusic.time = 0;
                stageMusic.clip = VictorySound;
                stageMusic.pitch = 1.45f;
                stageMusic.loop = false;
                stageMusic.Play();
            }
        }

    }

    void ControlTask()
    {
        switch (currentTask)
        {
            case 1:
                task01.enabled = true;
                break;
            case 2:
                task02.enabled = true;
                break;
            case 3:
                task03.enabled = true;
                break;
        }
    }

    void TrackTask()
    {
        switch (currentTask)
        {
            case 1:
                stageCompleted = task01.TaskStatus();
                break;
            case 2:
                stageCompleted = task02.TaskStatus();
                break;
            case 3:
                stageCompleted = task03.TaskStatus();
                break;
        }
    }

    void StageMusicSet()
    {
        stageMusic.clip = stageMusicSettings[character.Character()].music;
        stageMusic.volume = stageMusicSettings[character.Character()].volume;
        stageMusic.pitch = stageMusicSettings[character.Character()].pitch;
        stageMusic.PlayDelayed(0.5f);
    }

    [System.Serializable]
    public struct StageMusic
    {
        public AudioClip music;
        [Range(0,1)]public float volume;
        [Range(-3, 3)] public float pitch;
    }
}
