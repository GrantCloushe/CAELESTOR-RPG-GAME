using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterLoadingScreen : MonoBehaviour
{
    Transform cam;
    Vector3 cameraStartPoint;
    Vector3 cameraFinalPoint;
    [SerializeField] private float papyrusScale;
    [SerializeField] private Transform papyrusPivot;
    [SerializeField] private bool openPapyrus;
    [SerializeField] private AudioSource papyrusSFX;
    [SerializeField] private RawImage blackScreen;

    float alpha;
    float _timer;
    bool _playSound;

    void Start()
    {
        cam = Camera.main.GetComponent<Transform>();
        cameraStartPoint = new Vector3(cam.position.x, 9f, cam.position.z);
        cameraFinalPoint = new Vector3(cam.position.x, 11.5f, cam.position.z);
        cam.position = cameraStartPoint;
    }

    
    void Update()
    {
        _timer++;
        cam.position = Vector3.Lerp(cameraStartPoint, cameraFinalPoint, 0.5f * Time.time);
        papyrusPivot.localScale = new Vector3(1.35f, 1, papyrusScale);
        blackScreen.color = new Color(0, 0, 0, alpha);

        if(_timer > 30 && !openPapyrus)
        {
            openPapyrus = true;
        }

        if (openPapyrus)
        {
            if (!_playSound)
            {
                papyrusSFX.PlayOneShot(papyrusSFX.clip);
                _playSound = true;
            }

            if(papyrusScale < 1)
            {
                papyrusScale += 0.025f/3;
            }
        }

        if(_timer > 420)
        {
            alpha += 0.005f; 
        }
    }
}
