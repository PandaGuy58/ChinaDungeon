using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public float yRotation;
    public float finalYCoordinate;

    public bool quater;

    public bool pushUp;
    public bool pushAway;
    public bool launch;
    public bool release;

    public bool preventMovementX;
    public bool preventMovementZ;

    private void Start()
    {
        yRotation = gameObject.transform.localEulerAngles.y;
        if(!quater)
        {
            finalYCoordinate = transform.position.y + 0.9f;
        }
        else
        {
            finalYCoordinate = this.transform.position.y + 0.3f;
        }
    }
}
























    /*
 //   CameraController camControl;
    public float playerCentreYValue;

 //   CharacterController characterControl;

    public Vector3 pushTargetDirection;
 //   public Vector3 playerFaceDirection;

    SpringJoint joint;
    PlayerJointController playerJoint;


    void Start()
    {
 //       camControl = GameObject.Find("Main Camera").GetComponent<CameraController>();
  //  //    characterControl = GameObject.Find("Player").GetComponent<CharacterController>();
        pushTargetDirection = transform.forward;
        joint = GetComponent<SpringJoint>();
  //      playerFaceDirection = -pushTargetDirection;
        playerJoint = GameObject.Find("Player").GetComponent<PlayerJointController>();
    }

    public void ExecuteGrab(Rigidbody rb)                           // clamp player body + camera
    {
        joint.connectedBody = rb;
        playerJoint.ExecuteClamp(playerCentreYValue);
  //      characterControl.grabbing = true;
  //    camControl.grabbing = true;

    }

    public void FinaliseGrab()                          
    {
        joint.connectedBody = null; 
    //    camControl.playerGrabbing = false;
    }

}

    */