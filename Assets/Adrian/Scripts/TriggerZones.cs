using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZones : MonoBehaviour
{
    public bool cinemachine;
    public int zoneId;

    public float time;

    public bool animatedOnce;
    public bool disableLight;
    public bool endOfGame;

    float trapActivateTime = -1;

    public List<Vector3> startLocation = new List<Vector3>();
    public List<Vector3> endLocation = new List<Vector3>();
    public List<GameObject> trapObjects = new List<GameObject>();
    public List<AnimationCurve> trapCurves = new List<AnimationCurve>();
    public List<float> seconds = new List<float>();

    public GameObject lightSources;
    public GameObject playerGameObject;
    public GameObject hands;
    public GameObject MatCam;
    public GameObject AdrianCam;

    public AudioSource pressurePlate;
    public AudioSource bars;

    private void Start()
    {
        //GameObject.Find("Cam1").SetActive(false);
        animatedOnce = false;

        for (int i = 0; i < trapObjects.Count; i++)
        {
            startLocation[i] = trapObjects[i].transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (zoneId != 10)
            {
                MatCam.SetActive(false);
                AdrianCam.SetActive(true);
                GameObject.Find("GameMaster").GetComponent<FadeScreen>().fadeIn = true;
                GameObject.Find("GameMaster").GetComponent<FadeScreen>().once = true;
                GameObject.Find("GameMaster").GetComponent<FadeScreen>().activation = Time.time;
                GameObject.Find("GameMaster").GetComponent<GameMaster>().currentTriggerZone = this.gameObject;
                GameObject.Find("GameMaster").GetComponent<GameMaster>().playerActive = true;
                GameObject.Find("GameMaster").GetComponent<GameMaster>().darkScreenFade = true;
                GameObject.Find("Player").GetComponent<CharacterControllerNew>().enabled = false;
            }
            GameObject.Find("Player").GetComponent<CharacterControllerNew>().enabled = false;
            hands.SetActive(false);
            if (!animatedOnce)
            {
                GameMaster.activation = Time.time;
                trapActivateTime = Time.time;
                animatedOnce = true;

                GameObject.Find("GameMaster").GetComponent<GameMaster>().cameraId = zoneId;
                GameObject.Find("GameMaster").GetComponent<GameMaster>().cinemachineTime = time;

                if (zoneId == 10)
                {
                    endOfGame = true;
                }

                if (cinemachine && zoneId == 2)
                {
                    GameObject.Find("GameMaster").GetComponent<GameMaster>().cinemachineActive = true;
                    GameObject.Find("Cinemachine1").GetComponent<AnimatorRoom>().playAnim = true;
                    GameObject.Find("Cinemachine1Object").GetComponent<AnimatorRoom>().playAnim = true;
                }
                if (cinemachine && zoneId == 3)
                {
                    GameObject.Find("GameMaster").GetComponent<GameMaster>().cinemachineActive = true;
                    GameObject.Find("Cinemachine2").GetComponent<AnimatorRoom>().playAnim = true;
                    GameObject.Find("Cinemachine2Object").GetComponent<AnimatorRoom>().playAnim = true;
                }
                if (cinemachine && zoneId == 5)
                {
                    GameObject.Find("GameMaster").GetComponent<GameMaster>().cinemachineActive = true;
                    GameObject.Find("Cinemachine3").GetComponent<AnimatorRoom>().playAnim = true;
                    GameObject.Find("Cinemachine3Object").GetComponent<AnimatorRoom>().playAnim = true;
                }
                if (disableLight)
                {
                    lightSources.SetActive(false);
                }

            }

            if (zoneId != 10 && zoneId !=2 && zoneId != 3 && zoneId != 5)
            {
                if (!pressurePlate.isPlaying && pressurePlate != null)
                {
                    pressurePlate.Play();
                }

                if (!bars.isPlaying && bars != null)
                {
                    bars.Play();
                }
            }

            Debug.Log("EnterZone1");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("ExitZone1");
        }
    }

    void UpdateObjects()
    {
        for (int i = 0; i < trapObjects.Count; i++)
        {
            float timePassed = Time.time - trapActivateTime;
            float percentComplete = timePassed / seconds[i];
            float curveEvaluate = trapCurves[i].Evaluate(percentComplete);

            trapObjects[i].transform.position = Vector3.Lerp(startLocation[i], endLocation[i], curveEvaluate);

            if (percentComplete > 0.99f && i == trapObjects.Count - 1)
            {
                GameObject.Find("Player").GetComponent<CharacterControllerNew>().enabled = true;
                hands.SetActive(true);
                //GameObject.Find("Ground Check").GetComponent<CheckGround>().groundObjectsDetected = 0;
                //GameObject.Find("Ground Check").GetComponent<BodyCollisionCheck>().groundObjectsDetected = 0;
                //GameObject.Find("GameMaster").GetComponent<FadeScreen>().fadeIn = true;
                //GameObject.Find("GameMaster").GetComponent<FadeScreen>().once = true;
                //GameObject.Find("GameMaster").GetComponent<FadeScreen>().activation = Time.time;
                Destroy(this.gameObject);
            }

        }
    }

    private void Update()
    {
        if (trapActivateTime != -1)
        {
            UpdateObjects();
        }
    }
}
