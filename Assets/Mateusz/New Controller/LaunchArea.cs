using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchArea : MonoBehaviour
{
    public List<GameObject> nextGrabObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Edge"))
        {
            nextGrabObject.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        nextGrabObject.Remove(other.gameObject);
    }

}

// hello