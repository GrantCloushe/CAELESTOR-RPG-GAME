using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpToSceneTrigger : MonoBehaviour
{
    GameOverResult result;

    [SerializeField] private bool needToPress;
    public int goTo;
    bool activate;
    public string sceneName;

    void Start()
    {
        result = FindObjectOfType<GameOverResult>();

    }

  
    void Update()
    {
        if (activate)
        {
            GameOverResult.SceneEnd += Jump;
            Activate();
        }
    }

    public bool NeedPress()
    {
        return needToPress;
    }

    public void Activate()
    {
        activate = true;
        result.ChangeScene();
    }

    void Jump()
    {
        SceneManager.LoadScene(sceneName);
    }
}
