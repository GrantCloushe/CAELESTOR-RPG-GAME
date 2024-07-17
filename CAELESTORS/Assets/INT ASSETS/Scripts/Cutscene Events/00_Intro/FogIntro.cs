using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FogIntro : MonoBehaviour
{
    Image img;
    private float alpha;
    [SerializeField] private TextBox box;
    [SerializeField] private float alphaSpeed;

    void Start()
    {
        img = GetComponent<Image>();
        img.enabled = true;
        alpha = 1;
    }

    void Update()
    {
        alpha = alpha - alphaSpeed;
        img.color = new Color(1, 1, 1, alpha);

        if(alpha < 0.75f && alpha > 0)
        {
            box.BoxState(true);
        }
    }
}
