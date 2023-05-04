using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPushUp : MonoBehaviour
{
    public int pushUpCounter = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            pushUpCounter++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            pushUpCounter--;
        }
    }
}
