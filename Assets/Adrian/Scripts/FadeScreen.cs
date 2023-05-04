using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FadeScreen : MonoBehaviour
{
    public Image panel;

    public Color startColor;
    public Color endColor;

    public bool fadeIn;
    public bool fadeOut;
    public bool once;

    public float activation;

    public AnimationCurve fadeCurve;

    void Start()
    {
        once = false;
        fadeIn = false;
        fadeOut = false;
    }

    void Update()
    {
        if (once)
        {
            if (fadeIn)
            {
                float timePassed = Time.time - activation;
                float percentComplete = timePassed / 1;
                float curveEvaluate = fadeCurve.Evaluate(percentComplete);
                panel.color = Color.Lerp(startColor, endColor, curveEvaluate);

                if (percentComplete > 0.99f)
                {
                    fadeIn = false;
                    activation = Time.time;
                    fadeOut = true;
                }
            }

            if (fadeOut)
            {
                float timePassed = Time.time - activation;
                float percentComplete = timePassed / 2;
                float curveEvaluate = fadeCurve.Evaluate(percentComplete);
                panel.color = Color.Lerp(endColor, startColor, curveEvaluate);

                if (percentComplete > 0.99f)
                {
                    fadeOut = false;
                    once = false;
                }
            }
        }

    }
}
