using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushingWalls : MonoBehaviour
{
    public List<GameObject> currentlyCrushingWalls = new List<GameObject>();
    public List<AnimationCurve> trapCurves = new List<AnimationCurve>();
    public List<Vector3> startLocationPhaseOne = new List<Vector3>();
    public List<Vector3> endLocationPhaseOne = new List<Vector3>();
    public List<Vector3> startLocationPhaseTwo = new List<Vector3>();
    public List<Vector3> endLocationPhaseTwo = new List<Vector3>();
    public List<float> seconds = new List<float>();

    public float trapActivateTime = -1;
    public int phaseID;

    public bool once;
    public bool waiter;

    public float waiterCooldown;
    public static float waiterCooldownCounter;
    private void Start()
    {
        once = true;
        for (int i = 0; i < currentlyCrushingWalls.Count; i++)
        {
            startLocationPhaseOne[i] = currentlyCrushingWalls[i].transform.position;
            endLocationPhaseTwo[i] = currentlyCrushingWalls[i].transform.position;
        }
        phaseID = 1;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTheCurrentWall();
        Waiter();
    }

    void Waiter()
    {
        waiterCooldownCounter += Time.deltaTime;
        if (waiterCooldownCounter > waiterCooldown)
        {
            waiter = false;
        }
    }
    void UpdateTheCurrentWall()
    {
        switch(phaseID)
        {
            case 1:
                if (once && waiter == false)
                {
                    trapActivateTime = Time.time;
                    once = false;
                }
                if (trapActivateTime != -1 && waiter == false)
                {
                    for (int i = 0; i < currentlyCrushingWalls.Count; i++)
                    {
                        float timePassed = Time.time - trapActivateTime;
                        float percentComplete = timePassed / seconds[i];
                        float curveEvaluate = trapCurves[i].Evaluate(percentComplete);
                        //Debug.Log(percentComplete);
                        currentlyCrushingWalls[i].transform.position = Vector3.Lerp(startLocationPhaseOne[i], endLocationPhaseOne[i], curveEvaluate);

                        if (percentComplete > 0.99f)
                        {
                            phaseID = 2;
                            once = true;
                            waiter = true;
                            waiterCooldownCounter = 0;
                        }
                    }
                }
                break;
            case 2:
                if (once && waiter == false)
                {
                    trapActivateTime = Time.time;
                    once = false;
                }
                if (trapActivateTime != -1 && waiter == false)
                {
                    for (int i = 0; i < currentlyCrushingWalls.Count; i++)
                    {
                        float timePassed = Time.time - trapActivateTime;
                        float percentComplete = timePassed / seconds[i];
                        float curveEvaluate = trapCurves[i].Evaluate(percentComplete);
                        //Debug.Log(percentComplete);
                        currentlyCrushingWalls[i].transform.position = Vector3.Lerp(startLocationPhaseTwo[i], endLocationPhaseTwo[i], curveEvaluate);

                        if (percentComplete > 0.99f)
                        {
                            phaseID = 1;
                            once = true;
                            waiter = true;
                            waiterCooldownCounter = 0;
                        }
                    }
                }
                break;
        }
    }
}
