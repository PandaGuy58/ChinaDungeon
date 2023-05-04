using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrab : MonoBehaviour
{
    public bool hands;
    public bool grabDetected;
    
    public GrabObject grabObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Grab"))
        {
            grabDetected = true;
            if(hands)
            {
                grabObject = other.gameObject.GetComponent<GrabObject>();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Grab"))
        {
            grabDetected = false;
            if (hands)
            {
                grabObject = null;
            }
        }
    }
}
