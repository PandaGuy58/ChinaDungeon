using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioSource sound;

    public bool once;
    public bool oneTime;

    public float waiterCooldown;
    public static float waiterCooldownCounter;

    private void Start()
    {
        once = false;
    }

    private void Update()
    {
        Waiter();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!oneTime)
        {
            if (other.gameObject.name == "Upper Body")
            {
                if (!sound.isPlaying && sound != null && !once)
                {
                    sound.Play();
                    waiterCooldownCounter = 0;
                    once = true;
                }
            }
        }
        if (oneTime)
        {
            if (other.gameObject.name == "Upper Body")
            {
                if (!sound.isPlaying && sound != null && !once)
                {
                    sound.Play();
                    once = true;
                }
            }
        }
    }

    void Waiter()
    {
        waiterCooldownCounter += Time.deltaTime;
        if (waiterCooldownCounter > waiterCooldown)
        {
            once = false;
        }
    }
}
