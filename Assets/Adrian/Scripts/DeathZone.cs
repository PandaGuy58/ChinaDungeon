using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public static bool killable;

    private void Start()
    {
        killable = false;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Lower Body" || other.gameObject.name == "Upper Body")
        {
            Checkpoints.deathCooldownCounter = 0;
            killable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Lower Body" || other.gameObject.name == "Upper Body")
        {
            killable = false;
        }
    }
}
