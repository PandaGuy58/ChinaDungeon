using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Lighting")
        {
            Debug.Log("TESTE");
            other.gameObject.GetComponent<Torch>().inZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Lighting")
        {
            Debug.Log("TESTE");
            other.gameObject.GetComponent<Torch>().inZone = false;
        }
    }
}
