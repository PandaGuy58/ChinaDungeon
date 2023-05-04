using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    public Image darkScreen;

    public Color darkstartColor;
    public Color darkendColor;

    public bool fadeIn;
    public bool fadeOut;
    public bool once;
    public bool paused;
    public bool canClick;
    public bool onceCast;

    public float activation;

    public AnimationCurve fadeCurve;

    public GameObject buttons;
    public GameObject resumeButton;


    void Start()
    {
        buttons.SetActive(false);
        resumeButton.SetActive(false);
        darkScreen.color = darkstartColor;
        once = false;
        fadeIn = false;
        fadeOut = false;
        canClick = true;
        paused = false;
        onceCast = false;
    }

    void CheckIfResume()
    {
        if (GameObject.Find("End").GetComponent<TriggerZones>().endOfGame)
        {
            if (!onceCast)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                resumeButton.SetActive(false);
                activation = Time.time;
                once = true;
                paused = true;
                fadeIn = true;
                canClick = false;
                onceCast = true;
            }
        }
        else
        {
            resumeButton.SetActive(true);
        }
    }

    void Update()
    {
        CheckIfResume();
        if (canClick)
        {
            PauseControll();
        }
        if (paused && canClick || GameObject.Find("End").GetComponent<TriggerZones>().endOfGame && canClick)
        {
            ButtonsIfPaused();
        }

        if (once)
        {
            //Panel fade in
            if (fadeIn)
            {
                float timePassed = Time.time - activation;
                float percentComplete = timePassed / 1;
                float curveEvaluate = fadeCurve.Evaluate(percentComplete);
                darkScreen.color = Color.Lerp(darkstartColor, darkendColor, curveEvaluate);

                if (percentComplete > 0.99f)
                {
                    //if 99% done activate objects and stop the time
                    Time.timeScale = 0;
                    buttons.SetActive(true);
                    fadeIn = false;
                    once = false;
                    canClick = true;
                }
            }
            //Panel fade out
            if (fadeOut)
            {
                float timePassed = Time.time - activation;
                float percentComplete = timePassed / 2;
                float curveEvaluate = fadeCurve.Evaluate(percentComplete);
                darkScreen.color = Color.Lerp(darkendColor, darkstartColor, curveEvaluate);

                if (percentComplete > 0.5f)
                {
                    //if 50% done deactivate objects
                    buttons.SetActive(false);
                }

                if (percentComplete > 0.99f)
                {
                    fadeOut = false;
                    once = false;
                    canClick = true;
                }
            }
        }
    }

    void ButtonsIfPaused()
    {
        //Buttons logic
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "Exit":
                    //Exit
                    Application.Quit();
                    break;
                case "Play/Restart":
                    //Start
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                case "Resume":
                    //Resume
                    Debug.Log("ONE");
                    Time.timeScale = 1;
                    activation = Time.time;
                    once = true;
                    paused = false;
                    fadeOut = true;
                    canClick = false;
                    break;
            }
        }
    }

    //Controlling the pause by pressing escape
    void PauseControll()
    {
        if (!paused)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                activation = Time.time;
                once = true;
                paused = true;
                fadeIn = true;
                canClick = false;
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Escape) && !GameObject.Find("End").GetComponent<TriggerZones>().endOfGame)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                activation = Time.time;
                once = true;
                paused = false;
                fadeOut = true;
                canClick = false;
            }
        }
    }
}
