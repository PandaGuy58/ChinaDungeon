using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
  //  public int groundObjectsDetected = 0;

    public float timeLastGrounded;
    public bool grounded;
    public int groundObjectsDetected;
  //  CameraController controller;
    bool dropExecuted = false;

    //   private void Update()
    //   {

    //  }
    private void Start()
    {
     //   controller = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }
    private void Update()
    {
        if(Time.time > timeLastGrounded + 0.05f)
        {
            grounded = false;
            groundObjectsDetected = 0;
            dropExecuted = false;
        }
        else
        {
            grounded = true;
            groundObjectsDetected = 1;

            if(!dropExecuted)
            {
          //      controller.ExecuteCameraDrop();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //   groundObjectsDetected++;
            timeLastGrounded = Time.time;
        }
    }

}
/*
 *     CameraController controller;


    public int groundObjectsDetected = 0;
    // Start is called before the first frame update

    private void Start()
    {
        controller = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }

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
    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            groundObjectsDetected++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            groundObjectsDetected--;

            if(groundObjectsDetected == 0)
            {
                timeLastGrounded = Time.time;
            }
        }
    }
}
    */
