using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject cameraObject;

    public static bool falledToDeath;
    bool death;

    public static int checkpointID;
    public List<Vector3> checkpointPositions = new List<Vector3>();

    public float deathCooldown;
    public static float deathCooldownCounter;

    public AudioSource deathSound;

    void Start()
    {
        deathCooldownCounter = 0;
        checkpointID = -1;
        falledToDeath = false;
        death = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (DeathZone.killable)
        {
            KillAfterTime();
        }
        if(playerObject == null)
        {
            playerObject = GameObject.Find("Player");
        }
        if(falledToDeath || death)
        {
            if (!deathSound.isPlaying)
            {
                deathSound.Play();
            }
            GameObject.Find("GameMaster").GetComponent<FadeScreen>().fadeIn = true;
            GameObject.Find("GameMaster").GetComponent<FadeScreen>().once = true;
            GameObject.Find("GameMaster").GetComponent<FadeScreen>().activation = Time.time;
            playerObject.transform.position = checkpointPositions[checkpointID];
            falledToDeath = false;
            death = false;
        }
    }


    void KillAfterTime()
    {
        deathCooldownCounter += Time.deltaTime;
        if (deathCooldownCounter > deathCooldown)
        {
            GameObject.Find("GameMaster").GetComponent<FadeScreen>().fadeIn = true;
            GameObject.Find("GameMaster").GetComponent<FadeScreen>().once = true;
            GameObject.Find("GameMaster").GetComponent<FadeScreen>().activation = Time.time;
            death = true;
        }
    }
}
