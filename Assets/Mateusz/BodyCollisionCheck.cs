using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollisionCheck : MonoBehaviour
{

    CameraController controller;


    //  public int groundObjectsDetected = 0;
    // Start is called before the first frame update

    private void Start()
    {
        //     controller = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }
}
    /*

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            if (groundObjectsDetected == 0)
            {
                controller.ExecuteCameraDrop();
            }

            groundObjectsDetected++;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            groundObjectsDetected--;
        }
    }
}
    */