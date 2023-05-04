using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerJointController : MonoBehaviour
{

}
    /*
    //   bool grabDetected;
    bool grabbing;
    Rigidbody rb;
    TMP_Text tmpText;
    CharacterController charController;
    float wTimePress;
    float wTimeRequired = 0.5f;
    bool pushingUp;
    float finalCoordinate;

    CheckGrab leftHand;
    CheckGrab rightHand;
    CheckGrab handsArea;
    //  CheckGrab detectGrab;

    float spaceTimePress;
    float spaceTimeRequired = 0.5f;

    float eTimePressed;
    float eTimeRequired = 0.5f;

    public float pullUpForceForward;
    public float pullUpMaxVelocity;
    public float pullUpForce;

    public float pushForwardValue;
    public float pushUpValue;

    public GrabObject targetGrabObject;

    float timeMouseDown;
    float timeMouseDownRequired = 0.5f;

    Transform cameraObj;
    public float launchForwardValue;
    public float launchUpValue;

    float grabReleaseTime = 0;
    public float grabDelayTime = 1;

    float timeGrabbed = 0;
    public float actionsAfterGrabDelay = 1;
    Vector3 positionWhenGrabbed;

    public LaunchArea launchArea;

 //   int grabLayer = 7;

    RaycastHit[] raycastHitGrabObject;
    bool midLaunch = false;
    public Vector3 targetLaunchPosition;
    public Vector3 startPosition;

    public AnimationCurve launchCurve;
    float launchTime = -1;

    public GameObject upperBody;
    public GameObject lowerBody;

    private void Start()
    {
        rb = GameObject.Find("Player").GetComponent<Rigidbody>();
        tmpText = GameObject.Find("Text (TMP)").GetComponent<TMP_Text>();
        tmpText.text = "YOLO";
        charController = GetComponent<CharacterController>();

        leftHand = GameObject.Find("Left Hand").GetComponent<CheckGrab>();
        rightHand = GameObject.Find("Right Hand").GetComponent<CheckGrab>();
        handsArea = GameObject.Find("Hands").GetComponent<CheckGrab>();

        cameraObj = GameObject.Find("Main Camera").transform;
    }

    private void Update()
    {

        if(launchTime != -1)
        {
            Debug.Log(Time.time);
            float timePassed = Time.time - launchTime;
            //       timePassed *= 2;
            float calculate = launchCurve.Evaluate(timePassed);
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetLaunchPosition, calculate);
            transform.position = currentPosition;

         //   Debug.Log(timePassed);

            if(timePassed >= 1)
            {
                launchTime = -1;
                midLaunch = false;

        //        upperBody.SetActive(true);
        //        lowerBody.SetActive(true);

            }
        }

        RaycastGrabObject();

        if (pushingUp)                                                                         // push up 
        {
            float velocityUp = rb.velocity.y;
            if (rb.transform.position.y >= finalCoordinate)                                         // once heigt reached  >  push player forward
            {
                rb.AddForce(transform.forward * pullUpForceForward);
                grabbing = false;
                grabReleaseTime = Time.time;
                charController.grabbing = false;
                pushingUp = false;
            }
            else if (velocityUp < pullUpMaxVelocity)                                                          // push up  >  maintain velocity                                        
            {
                rb.AddForce(transform.up * pullUpForce * Time.deltaTime);                                    // player UP FORCE WHEN Push UP
            }
        }
        else if (!grabbing && handsArea.grabDetected && Time.time > grabReleaseTime + grabDelayTime)             // grab on to edges by default
        {
            targetGrabObject = handsArea.grabObject;
            grabbing = true;
            charController.grabbing = true;
            charController.targetYRotate = targetGrabObject.yRotation;
            finalCoordinate = targetGrabObject.finalYCoordinate;
            timeGrabbed = Time.time;
            positionWhenGrabbed = rb.transform.position;
            rb.velocity = new Vector3(0, 0, 0);
        }
        else if (grabbing && Time.time < timeGrabbed + actionsAfterGrabDelay)
        {

            float timePassed = Time.time - timeGrabbed;
            float percentage = timePassed / 1;
            float evaluate = curve.Evaluate(percentage);

            float targetY = targetGrabObject.transform.position.y;
            Vector3 targetPos = transform.position;
            targetPos.y = targetY;

            Vector3 initialPos = transform.position;
            initialPos.y = positionWhenGrabbed.y;

            Vector3 calculatePosition = Vector3.Lerp(initialPos, targetPos, evaluate);
            transform.position = calculatePosition;
            rb.AddForce(transform.forward * 10 * Time.deltaTime);
      
        }
        else if (grabbing)
        {
           // Debug.Log("grabbing");
        //    upperBody.SetActive(false);
        //    lowerBody.SetActive(false);
            // Debug.Log(Time.time);
            tmpText.text = "Hold W - Push UP" + "\n" + "A/D - Sideways" + "\n" + "Hold Space - Push Away" + "\n" + "Left Mouse - Launch in Camera Direction" +
                            "\n" + "E - Release";

            bool enableLaunch = false;
            if (!midLaunch)
            {
                for (int i = 0; i < raycastHitGrabObject.Length; i++)
                {
                    GameObject temp = raycastHitGrabObject[i].transform.gameObject;
                    if (launchArea.nextGrabObject.Contains(temp))
                    {
                        if (temp == targetGrabObject.gameObject)
                        {

                        }
                        else
                        {
                            enableLaunch = true;
                            targetLaunchPosition = raycastHitGrabObject[i].transform.position;
                            targetLaunchPosition.y -= 1;

                            if (targetGrabObject.preventMovementX)
                            {
                                targetLaunchPosition.x = transform.position.x;
                            }
                            else
                            {
                                targetLaunchPosition.z = transform.position.z;
                            }
                        }

                    }
                }
            }


            if (Input.GetMouseButtonDown(0) && targetGrabObject.launch && enableLaunch)
            {
                midLaunch = true;
                timeMouseDown = Time.time;
                startPosition = gameObject.transform.position;

            }
            else if (Input.GetMouseButton(0) && Time.time > timeMouseDown + timeMouseDownRequired && targetGrabObject.launch && midLaunch)
            {
                
                launchTime = Time.time;

                grabbing = false;
                grabReleaseTime = Time.time;
                charController.grabbing = false;



                Debug.Log(Time.time);

                Debug.Log(Time.time);
                Vector3 targetDirection = cameraObj.forward * 1.5f;                // * launchForwardValue;
                Vector3 upDirection = transform.up * 1.5f;                           //   * launchUpValue;

                Vector3 calculateDirection = targetDirection + upDirection;
                calculateDirection = calculateDirection.normalized;
                calculateDirection *= 250;

                rb.AddForce(calculateDirection);

                grabbing = false;
                grabReleaseTime = Time.time;
                charController.grabbing = false;
           
            }
            else if (Input.GetKeyDown(KeyCode.W) && targetGrabObject.pushUp)
            {
                wTimePress = Time.time;
            }
            else if (Input.GetKey(KeyCode.W) && Time.time > wTimePress + wTimeRequired && targetGrabObject.pushUp)
            {
                pushingUp = true;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && targetGrabObject.pushAway)                                                            // push away
            {
                spaceTimePress = Time.time;
            }
            else if (Input.GetKey(KeyCode.Space) && Time.time > spaceTimePress + spaceTimeRequired && targetGrabObject.pushAway)
            {
                Vector3 targetDirection = transform.forward * -1 * pushForwardValue;
                Vector3 upDirection = transform.up * pushUpValue;

                rb.AddForce(targetDirection);
                rb.AddForce(upDirection);

                grabbing = false;
                grabReleaseTime = Time.time;
                charController.grabbing = false;
            }
            else if (Input.GetKey(KeyCode.A) && leftHand.grabDetected)
            {
                rb.velocity = (transform.right * -3);
            }
            else if (Input.GetKey(KeyCode.D) && rightHand.grabDetected)
            {
                rb.velocity = (transform.right * 3);
            }
            else if (Input.GetKeyDown(KeyCode.E) && targetGrabObject.release)
            {
                eTimePressed = Time.time;
            }
            else if (Input.GetKey(KeyCode.E) && Time.time > eTimePressed + eTimeRequired && targetGrabObject.release)
            {
                grabbing = false;
                grabReleaseTime = Time.time;
                charController.grabbing = false;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            tmpText.text = "";
        }
    }

    void RaycastGrabObject()
    {
        Camera cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        raycastHitGrabObject = Physics.RaycastAll(ray, 100);
    }
}
  
        Ray ray;
        RaycastHit hit;

        ray = Camera.main.ScreenPointToRay(new Vector3(0,0,0));
        if(Physics.Raycast(ray, out hit, 100, grabLayer))
        {
            if(hit.transform.gameObject.CompareTag("Grab"))
            {
                Debug.Log(Time.time);
            }
        }
    }
}
         */
 














    /*
    private void Update()
    {
        if (pushingUp)
        {
            float velocityUp = rb.velocity.y;
            if (rb.transform.position.y >= finalCoordinate)
            {
                rb.AddForce(rb.transform.forward * pullUpForceForward);
                grabbing = false;
                charController.grabbing = false;
                pushingUp = false;
            }
            else if (velocityUp < pullUpMaxVelocity)
            {
                rb.AddForce(rb.transform.up * pullUpForce * Time.deltaTime);                                    // player UP FORCE WHEN Push UP
            }

        }
        else if (!grabbing && grabObject != null)
        {
            Debug.Log(Time.time);
            tmpText.text = "E - Grab";
            if(Input.GetKeyDown(KeyCode.E))
            {
                grabbing = true;
                charController.grabbing = true;
                charController.targetYRotate = grabObject.yRotation;
            }

            /*
            grabObject = detectGrab.grabObject;
            finalCoordinate = grabObject.finalYCoordinate;
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                grabbing = true;
                Debug.Log(charController);
             //   charController.grabbing = true;
                Debug.Log(grabObject);
            //    charController.targetYRotate = grabObject.yRotation;
                
            }

        }
        else if (grabbing)
        {
            tmpText.text = "Hold W - Push UP" + "\n" + "A/D - Sideways" + "\n" + "Hold Space - Push Away";

            if (Input.GetKeyDown(KeyCode.W))
            {
                wTimePress = Time.time;
            }
            else if (Input.GetKey(KeyCode.W) && Time.time > wTimePress + wTimeRequired)
            {
                pushingUp = true;
            }
            else if (Input.GetKeyDown(KeyCode.Space))                                                            // push away
            {
                spaceTimePress = Time.time;
            }
            else if (Input.GetKey(KeyCode.Space) && Time.time > spaceTimePress + spaceTimeRequired)
            {
                Debug.Log(pushForwardValue);
                Vector3 targetDirection = rb.transform.forward * -1 * pushForwardValue;
                Vector3 upDirection = rb.transform.up * pushUpValue;
                grabbing = false;
                grabObject = null;
                charController.grabbing = false;
                //
                rb.AddForce(targetDirection);
                rb.AddForce(upDirection);
          //      charController.timeJumpAllowed = Time.time + 10;
            }
            else if (Input.GetKey(KeyCode.A) && leftHand.grabDetected)
            {
                rb.velocity = (rb.transform.right * -3);
            }
            else if (Input.GetKey(KeyCode.D) && rightHand.grabDetected)
            {
                rb.velocity = (rb.transform.right * 3);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            tmpText.text = "";
        }
    }
}

*/

/*
private void OnTriggerEnter(Collider other)
{
    if(other.gameObject.CompareTag("Grab"))
    {
        grabDetected = true;
        grabObject = other.gameObject.GetComponent<GrabObject>();
    }
}
private void OnTriggerExit(Collider other)
{
    if (other.gameObject.CompareTag("Grab"))
    {
        grabDetected = false;
    }
}


}




*/













/*
SpringJoint joint;
bool grabDetected;
public bool grabbing;
public LayerMask grabLayerMask;
public TMP_Text tmpText;
Rigidbody rb;
public GrabObject grabObject;
float allowedRotationChange = 90;

CharacterController characterController;
CameraController cameraController;
// Start is called before the first frame update
void Start()
{

    tmpText = GameObject.Find("Text (TMP)").GetComponent<TMP_Text>();
    tmpText.text = "YOLO";
    rb = gameObject.GetComponent<Rigidbody>();
    characterController = gameObject.GetComponent<CharacterController>();
    cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
}

private void Update()
{
    RaycastGrab();
    if (grabDetected && Input.GetKeyDown(KeyCode.E))
    {
        grabObject.ExecuteGrab(rb);
    //    ConnectJoint();
    // grabbing = true;
    }
    else if(grabbing && Input.GetKeyDown(KeyCode.E))
    {
        // release the grab
    }

    if(!grabbing && grabDetected)
    {
        tmpText.text = "E - Grab";
    }
    else if(grabbing)
    {
        tmpText.text = "E - Release" +"\n" + "Space to jump in any direction" + "\n" + "Space x2 to push away";
    }
    else if(!grabDetected)
    {
        tmpText.text = "";
    }
}

// Update is called once per frame

public void ExecuteClamp(float playerCentreYValue)
{
    grabbing = true;
    float calculateMaxY = playerCentreYValue + allowedRotationChange;
    float calculateMinY = playerCentreYValue - allowedRotationChange;

//    Debug.Log(calculateMaxY);
//    Debug.Log(calculateMinY);

//      characterController.InitialiseGrab(calculateMaxY, calculateMinY);
//      cameraController.InitialiseGrab(calculateMaxY, calculateMinY);

}

public void DisconnectJoint()
{
    grabbing = false;
    grabObject.FinaliseGrab();
}



void RaycastGrab()
{
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 1, grabLayerMask))
    {
        joint = hit.transform.gameObject.GetComponent<SpringJoint>();
        grabObject = hit.transform.gameObject.GetComponent<GrabObject>(); 
        grabDetected = true;
    }
    else
    {
        grabDetected = false;
    }
}
}

*/