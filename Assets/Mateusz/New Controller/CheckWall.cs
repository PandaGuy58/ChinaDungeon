using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWall : MonoBehaviour
{
    public int wallObjectsDetected = 0;


    public CharacterControllerNew cController;

    private void Start()
    {
   //     cController = GameObject.Find("Player").GetComponent<CharacterController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            wallObjectsDetected++;
            WallMechanic wallMechanic = other.gameObject.GetComponent<WallMechanic>();

            Vector3 jumpDirection = wallMechanic.jumpDirection;
            cController.EnableWallRun(true);
            cController.wallJumpDirection = jumpDirection;

            bool travelX = wallMechanic.travelX;
            bool travelZ = wallMechanic.travelZ;

            cController.wallRunTravelX = travelX;
            cController.wallRunTravelZ = travelZ;

            cController.wallTransform = wallMechanic.gameObject.transform;
            cController.alternativeCamera = wallMechanic.alternative;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            wallObjectsDetected--;
            cController.EnableWallRun(false);

            cController.wallRunTravelX = false;
            cController.wallRunTravelZ = false;
        }
    }
}







        /*
    {
        if (other.gameObject.CompareTag("Wall"))                              // pass jump direction coordinate to character controller                                                                        
        {
            wallObjectsDetected++;
            Vector3 jumpDirection = other.gameObject.GetComponent<WallMechanic>().jumpDirection;
            cController.wallJumpDirection = jumpDirection;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            wallObjectsDetected--;
        }
    }
}

        */