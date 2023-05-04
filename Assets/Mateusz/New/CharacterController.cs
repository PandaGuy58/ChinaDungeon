using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Rigidbody rb;

    public AnimationCurve walkCurve;
    public AnimationCurve runCurve;

    CheckGround groundCheckScript;

    bool jump = false;

    int forwardInput = 0;
    int horizontalInput = 0;


    float timeJumped = -5;

    public bool movingForward = false;

    bool running = false;

    public float walkingVelocity = 5;
    public float runningVelocity = 5;

    public float horizontalValue = 0;
    public float forwardValue = 0;
    public float runningValue = 0;


    float velocityChange = 0.1f;
    float runningChange = 0.075f;

    Vector3 forwardVector;
    Vector3 rightVector;

    Vector3 runForwardVector;
    Vector3 runRightVector;

    Vector3 move;


  //  GameObject mainCamera;


    public Vector3 wallJumpDirection;

    float[] timeSpaceDown = new float[2] { -1, -1 };

    CameraController camController;

    Vector3 rotationCurrent = Vector3.zero;
    public bool grabbing = false;


    float rotationCurrentY = 0;


    public float targetYRotate;

    public float wallRunTimeLeft;


    public float wallRunTotalTimeAllowed = 2;

    public float gravityValue = 10;

    public float forwardJumpValue;
    public float upwardJumpValue;

    public float wallRunJumpForwardValue;
    public float wallRunJumpUpValue;
    public float wallRunForwardValue;
    public float wallRunDownwardValue;
  //  public float wallRunUpValue;

    public float midAirForce = 1.2f;


    public float jumpWindowAllowed = 0.125f;

    //    AnimationController animControl;

    public bool wallRunTravelX = false;
    public bool wallRunTravelZ = false;
    public Transform wallTransform;

    public bool alternativeCamera;
  //  float recentHorizontalMouse;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheckScript = GameObject.Find("Ground Check").GetComponent<CheckGround>();
        GameObject mainCamera = GameObject.Find("Main Camera");
        camController = mainCamera.GetComponent<CameraController>();
    }

    private void Update()
    {
        Inputs();

        if (!grabbing)
        {
            Rotating();
        }



        if(groundCheckScript.groundObjectsDetected == 0)
        {

            camController.wallX = wallRunTravelX;
            camController.wallZ = wallRunTravelZ;
        }
    }

    private void FixedUpdate()
    {
        CalculateVelocityValues();

        if (!grabbing)
        {
            //  animControl.RequestAnimation(0);
            
            if (DetectWallRunning())
            {
                WallRun();                                                  // wall running
            }
            else if (groundCheckScript.groundObjectsDetected > 0)
            {
                rb.AddForce(new Vector3(0, gravityValue, 0));                // gravity
                if (jump && Time.time > timeJumped + 1)                     // jump on ground
                {
                    Jump();
                }
                else
                {
                    ExecuteMovementOnGround();                              // regular movement on ground
                }
            }
            else if (movingForward)                                          // when not grounded  >  player pushes mid air
            {
                rb.AddForce(new Vector3(0, gravityValue, 0));                // gravity
                if (groundCheckScript.timeLastGrounded + jumpWindowAllowed > Time.time && jump && Time.time > timeJumped + 1)
                {
                    Jump();
                }
                ExecuteMovementMidAir();
            }
            else
            {
                rb.AddForce(new Vector3(0, gravityValue, 0));                // gravity
                PlayerFall();
            }
        }
        else                                       // grab mechanics
        {
            Grabbing();
            //   animControl.RequestAnimation(1);
        }
    }
    bool DetectWallRunning()
    {

            if (wallRunTimeLeft > 0 && movingForward && running && groundCheckScript.groundObjectsDetected == 0)
            {
               return true;
            }
        return false;
    }

    void Rotating()
    {
        rotationCurrent.y = camController.horizontalMouse;
        transform.localRotation = Quaternion.Euler(rotationCurrent);
    }
        
    void CalculateVelocityValues()
    {
        if (movingForward && running)
        {
            runningValue += runningChange;
            if (runningValue > 1)
            {
                runningValue = 1;
            }
        }
        else
        {
            runningValue -= runningChange;
            if (runningValue < 0)
            {
                runningValue = 0;
            }
        }

        if (horizontalInput == -1)
        {
            horizontalValue -= velocityChange;
            if (horizontalValue < -1)
            {
                horizontalValue = -1;
            }
        }
        else if (horizontalInput == 1)
        {
            horizontalValue += velocityChange;
            if (horizontalValue > 1)
            {
                horizontalValue = 1;
            }
        }
        else
        {
            if (horizontalValue > 0)
            {
                horizontalValue -= velocityChange;
                if (horizontalValue < 0)
                {
                    horizontalValue = 0;
                }
            }
            else if (horizontalValue < 0)
            {
                horizontalValue += velocityChange;
                if (horizontalValue > 0)
                {
                    horizontalValue = 0;
                }
            }
        }


        if (forwardInput == 1)
        {
            forwardValue += velocityChange;
            if (forwardValue > 1)
            {
                forwardValue = 1;
            }
        }
        else if (forwardInput == -1)
        {
            forwardValue -= velocityChange;
            if (forwardValue < -1)
            {
                forwardValue = -1;
            }
        }
        else
        {
            if (forwardValue > 0)
            {
                forwardValue -= velocityChange;
                if (forwardValue < 0)
                {
                    forwardValue = 0;
                }
            }
            else if (forwardValue < 0)
            {
                forwardValue += velocityChange;
                if (forwardValue > 0)
                {
                    forwardValue = 0;
                }
            }
        }
    }

    void Inputs()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            running = false;
        }
        else
        {
            running = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            timeSpaceDown[0] = timeSpaceDown[1];            // update space time array
            timeSpaceDown[1] = Time.time;
        }

        if (Input.GetKey(KeyCode.A))             // left and right
        {
            horizontalInput = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1;
        }
        else
        {
            horizontalInput = 0;
        }

        if (Input.GetKey(KeyCode.W))             // back and forth
        {
            forwardInput = 1;
            movingForward = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forwardInput = -1;
            movingForward = false;
        }
        else
        {
            forwardInput = 0;
            movingForward = false;
        }
    }

    void ExecuteMovementOnGround()
    {
        if (Time.time > timeJumped + 0.25f)
        {
            jump = false;
            float horizontalCalculate = 0;
            float forwardCalculate = 0;
            float runCalculate = 0;

            if (horizontalValue < 0)
            {
                horizontalCalculate = walkCurve.Evaluate(-horizontalValue);
                horizontalCalculate = -horizontalCalculate;
            }
            else if (horizontalValue > 0)
            {
                horizontalCalculate = walkCurve.Evaluate(horizontalValue);
            }

            if (forwardValue < 0)
            {
                forwardCalculate = walkCurve.Evaluate(-forwardValue);
                forwardCalculate = -forwardCalculate;
            }
            else if (forwardValue > 0)
            {
                forwardCalculate = walkCurve.Evaluate(forwardValue);
            }

            float tempForwardCalclate = forwardCalculate;
            float tempHorizonCalculate = horizontalCalculate;

            if (tempForwardCalclate < 0)
            {
                tempForwardCalclate = -tempForwardCalclate;
            }

            if (horizontalCalculate < 0)
            {
                tempHorizonCalculate = -tempHorizonCalculate;
            }

            if (tempHorizonCalculate + tempForwardCalclate > 1)
            {
                Vector2 toNormalise;
                toNormalise.x = horizontalCalculate;
                toNormalise.y = forwardCalculate;
                toNormalise = toNormalise.normalized;

                horizontalCalculate = toNormalise.x;
                forwardCalculate = toNormalise.y;
            }

            if (runningValue != 0)
            {
                runCalculate = runCurve.Evaluate(runningValue);
            }

            rightVector = transform.right * horizontalCalculate * walkingVelocity;
            forwardVector = transform.forward * forwardCalculate * walkingVelocity;

            runForwardVector = transform.forward * runCalculate * runningVelocity * forwardCalculate;
            runRightVector = transform.right * runCalculate * runningVelocity * horizontalCalculate;

            move = rightVector + forwardVector + runForwardVector + runRightVector;
            rb.velocity = move;
        }
    }

    void ExecuteMovementMidAir()
    {
        Vector3 move = transform.forward * forwardValue * midAirForce;
        rb.AddForce(move);
    }

    void PlayerFall()
    {
        Vector3 currentVelocity = rb.velocity;
        currentVelocity.x = currentVelocity.x / 1.05f;
        currentVelocity.z = currentVelocity.z / 1.05f;

        rb.velocity = currentVelocity;
        rb.AddForce(new Vector3(0, -1, 0));
    }

    void Jump()
    {
        jump = false;
        if (Time.time > timeJumped + 1)
        {
            timeJumped = Time.time;
            Vector3 move = transform.forward * forwardValue * forwardJumpValue + transform.up * upwardJumpValue;
            rb.AddForce(move);
        }
    }

    void Grabbing()
    {
        rotationCurrentY = Mathf.Lerp(rotationCurrentY, targetYRotate, 0.5f);
        rotationCurrent.y = rotationCurrentY;
        transform.localRotation = Quaternion.Euler(rotationCurrent);
    }

    void WallRun()
    {
        if (jump)
        {
            jump = false;
            Vector3 jumpDirection = wallJumpDirection * wallRunJumpForwardValue;
            Vector3 upDirection = transform.up * wallRunJumpUpValue;

            rb.AddForce(jumpDirection + upDirection);
            wallRunTimeLeft = 0;
        }
        else
        {
            
            if (groundCheckScript.groundObjectsDetected == 0)
            {
                wallRunTimeLeft -= 0.02f;
            }

            Vector3 targetDirection = Vector3.zero;
            if (wallRunTravelX)
            {
                Debug.Log(Time.time);
                if (camController.horizontalTheoreticalMouse < 310 && camController.horizontalTheoreticalMouse > 230)
                {
                    targetDirection = wallTransform.forward * -wallRunForwardValue + transform.up * -wallRunDownwardValue;
                }
                else if(camController.horizontalTheoreticalMouse < 130 && camController.horizontalTheoreticalMouse > 50)
                {
                    targetDirection = wallTransform.forward * wallRunForwardValue + transform.up * -wallRunDownwardValue;
                }
            }
            else if(wallRunTravelZ)
            {

                if (camController.horizontalTheoreticalMouse < 40 && camController.horizontalTheoreticalMouse > 0)
                {
                    targetDirection = wallTransform.forward * wallRunForwardValue + transform.up * -wallRunDownwardValue;
                }
                else if(camController.horizontalTheoreticalMouse > 320 && camController.horizontalTheoreticalMouse < 360)
                {
                    targetDirection = wallTransform.forward * wallRunForwardValue + transform.up * -wallRunDownwardValue;
                }
                else if(camController.horizontalTheoreticalMouse == 0)
                {
                    targetDirection = wallTransform.forward * wallRunForwardValue + transform.up * -wallRunDownwardValue;
                }

                if(camController.horizontalTheoreticalMouse > 140 && camController.horizontalTheoreticalMouse < 220)
                {

                    Debug.Log("here3");
                    targetDirection = wallTransform.forward * -wallRunForwardValue + transform.up * -wallRunDownwardValue;
                }

            }
            rb.velocity = targetDirection;
        }
    }

    public void EnableWallRun(bool state)
    {
        if (state)
        {
            wallRunTimeLeft = wallRunTotalTimeAllowed;
        }
        else
        {
            wallRunTimeLeft = 0;
        }
    }                                  
}











    /*
    public void GrabbingClamp(bool positiveChange, bool negativeChange)
    {
        float horizontalMouse = 0;
        if (clampMax > 360)
        {
            float calculateClamp = clampMax - 360;

            if (positiveChange)
            {
                if (horizontalMouse > calculateClamp && horizontalMouse < clampMin)
                {
                    horizontalMouse = calculateClamp;
                }
            }
            else if (negativeChange)
            {
                if (horizontalMouse > calculateClamp && horizontalMouse < clampMin)
                {
                    horizontalMouse = clampMin;
                }
            }
        }
        else if (clampMin < 0)
        {
            float calculateClamp = 360 + clampMin;

            if (negativeChange)
            {
                if (horizontalMouse < calculateClamp && horizontalMouse > clampMax)
                {
                    horizontalMouse = calculateClamp;
                }
            }
            else if (positiveChange)
            {
                if (horizontalMouse < calculateClamp && horizontalMouse > clampMax)
                {
                    horizontalMouse = clampMax;
                }
            }
        }
        else
        {
            if (horizontalMouse > clampMax)
            {
                horizontalMouse = clampMax;
            }
            else if (horizontalMouse < clampMin)
            {
                horizontalMouse = clampMin;
            }
        }
    }
}

    */



/*     Vector3 currentPosition = transform.position;

     if (oldPosition == Vector3.zero)
     {
         oldPosition = currentPosition;
         calculatedDistance = 0;
     }
     else
     {
         calculatedDistance = Vector3.Distance(oldPosition, currentPosition);
         oldPosition = currentPosition;
     }

 //    if (calculatedDistance < 17.5f && running)
 //    {
    //     Vector3 targetDirection = mainCamera.transform.forward;
   //      targetDirection = targetDirection * forwardValue * wallRunForward;
    //     targetDirection += transform.up * wallRunUpward;
         rb.AddForce(targetDirection);
 //    }     


 }

     */









/*

void GrabbingActions()
{
    if (pullingUpDetected)
    {

    }

    bool doubleSpace = false;
    if (timeSpaceDown[1] + 0.05f > Time.time)
    {
        float timeDifferenceBetweenSpace = timeSpaceDown[1] - timeSpaceDown[0];
        if (timeDifferenceBetweenSpace < timeSpaceDelay)
        {
            doubleSpace = true;
        }
    }

    if (spaceDown && timeSpaceOver < Time.time)
    {
        PushAway();
    }
    else if (doubleSpace)                               // space pressed twice
    {
        PushInCameraDirection();
    }
}

*/








/*



void PushAway()
{
    /*
    Vector3 targetDirection = playerJoint.grabObject.pushTargetDirection + transform.up;
    targetDirection *= 500;
    rb.AddForce(targetDirection);
    playerJoint.DisconnectJoint();
    camController.playerGrabbing = false;


}

void PushInCameraDirection()
{
    /*
    Vector3 targetDirection = mainCamera.transform.forward;
    targetDirection *= 400;
    rb.AddForce(targetDirection);
    playerJoint.DisconnectJoint();
    camController.playerGrabbing = false;

}

public void InitialiseGrab(float maxY, float minY)
{
//    Debug.Log("initialise grab character");
    grabbing = true;
    clampMax = maxY;
    clampMin = minY;
}

public void FinaliseGrab()
{
    grabbing = true;
}
*/








//    public void InitialiseGrab()
//   {

//  }

//   public void FinaliseGrab()
//  {

//  }


























/*
CalculateVelocityValues();

if(grabbing)
{
  GrabbingActions();
}
else if (groundCheckScript.groundObjectsDetected > 0)
{
  wallRunTime = 0;
  if (jump)
  {
      Jump();
  }
  else
  {

  }

}
else
{
  if(wallCheckScript.wallObjectsDetected > 0)             // && Time.time < timeWallRunBegan + wallRunTotalTime)
  {
      WallRun();
  }
  else if(movingForward)
  {
      ExecuteMovementMidAir();
  }
  else
  {
      PlayerFall();
  }
}
}
*/







/*





/// <summary>
/// ////////////////// old code for reference
/// </summary>

void StandingState()
{
    CalculateTransitionProgress();

    if (transitionProgress > 0)
    {
        playerUpperBody.localPosition = new Vector3(0, 1, 0);

        float percentComplete = transitionProgress / 0.5f;
  //      float curveEvaluate = sCurve.Evaluate(percentComplete);
    //    currentVelocity = Mathf.Lerp(0, 5, curveEvaluate);

  //      if (curveEvaluate == 1)
        {
            currentState = 1;
            transitionProgress = 0;
        }
    }
    else if (transitionProgress < 0)
    {
        currentVelocity = 0;

        float percentComplete = transitionProgress / -0.5f;
    //    float curveEvaluate = sCurve.Evaluate(percentComplete);
    //    float currentHeight = Mathf.Lerp(1, 0, curveEvaluate);

   //     playerUpperBody.localPosition = new Vector3(0, currentHeight, 0);

//         if(curveEvaluate == 1)
        {
            currentState = -1;
            transitionProgress = 0;
        }
    }
    else
    {
        currentVelocity = 0;
        playerUpperBody.localPosition = new Vector3(0, 1, 0);
    }
}

void WalkState()
{
    CalculateTransitionProgress();

    if (transitionProgress > 0)
    {
        float percentComplete = transitionProgress / 0.5f;
      //  float curveEvaluate = sCurve.Evaluate(percentComplete);
    //    currentVelocity = Mathf.Lerp(5, 10, curveEvaluate);

  //      if (curveEvaluate == 1)
        {
            currentState = 2;
            transitionProgress = 0;
        }
    }
    else if(transitionProgress < 0)
    {
        float percentComplete = transitionProgress / -0.5f;
      //  float curveEvaluate = sCurve.Evaluate(percentComplete);
//           currentVelocity = Mathf.Lerp(5,0, curveEvaluate);

    //    if(curveEvaluate == 1)
        {
            currentState = 0;
            transitionProgress = 0;
        }
    }
    else
    {
        currentVelocity = 5;
    }
}

void CrouchState()
{
    CalculateTransitionProgress();

    if (transitionProgress > 0)
    {
        float percentComplete = transitionProgress / 0.5f;
      //  float curveEvaluate = sCurve.Evaluate(percentComplete);
   //     float currentHeight = Mathf.Lerp(0, 1, curveEvaluate);

  //      playerUpperBody.localPosition = new Vector3(0, currentHeight, 0);

   //     if (curveEvaluate == 1)
        {
            currentState = 0;
            transitionProgress = 0;
        }
    }
    else if (transitionProgress < 0)
    {
        float percentComplete = transitionProgress / -0.5f;
   //     float curveEvaluate = sCurve.Evaluate(percentComplete);

   //     currentVelocity = Mathf.Lerp(0, 2.5f, curveEvaluate);

//         if(curveEvaluate == 1)
        {
            currentState = -2;
            transitionProgress = 0;
        }
    }
    else
    {
        playerUpperBody.localPosition = new Vector3(0, 0, 0);
    }
}

void CrouchWalkState()
{
    CalculateTransitionProgress();

    if(transitionProgress > 0)
    {
        float percentComplete = transitionProgress / 0.5f;
    //    float curveEvaluate = sCurve.Evaluate(percentComplete);

   //     currentVelocity = Mathf.Lerp(2.5f, 0, curveEvaluate);

    //    if (curveEvaluate == 1)
        {
            currentState = -1;
            transitionProgress = 0;
        }
    }
    else
    {
        currentVelocity = 2.5f;
    }
}

void RunState()
{
    CalculateTransitionProgress();

    if (transitionProgress < 0)
    {
        float percentComplete = transitionProgress / -0.5f;
    //    float curveEvaluate = sCurve.Evaluate(percentComplete);

  //      currentVelocity = Mathf.Lerp(10, 5, curveEvaluate);

    //    if(curveEvaluate == 1)
        {
            currentState = 1;
            transitionProgress = 0;
        }
    }
    else
    {
        currentVelocity = 10;
    }
}


void CalculateTransitionProgress()
{
    if (targetState > currentState)
    {
        transitionProgress += Time.deltaTime;
    }
    else if (targetState < currentState)
    {
        transitionProgress -= Time.deltaTime;
    }
    else
    {
        if (transitionProgress < 0)
        {
            transitionProgress += Time.deltaTime;
            if (transitionProgress > 0)
            {
                transitionProgress = 0;
            }
        }
        else if (transitionProgress > 0)
        {
            transitionProgress -= Time.deltaTime;
            if (transitionProgress < 0)
            {
                transitionProgress = 0;
            }
        }
    }
}
}


*/



/*
       horizontalMouse += Input.GetAxis("Mouse X") * 2;

       bool changePositive = false;
       bool changeNegative = false;

       if (Input.GetAxis("Mouse X") > 0)
       {
           changePositive = true;
       }
       else if (Input.GetAxis("Mouse X") < 0)
       {
           changeNegative = true;
       }

       float difference;
       if (horizontalMouse > 360)
       {
           difference = horizontalMouse - 360;
           horizontalMouse = difference;
       }
       else if (horizontalMouse < 0)
       {
           difference = 360 - horizontalMouse;
           horizontalMouse = difference;
       }

       if (grabbing)
       {
           GrabbingClamp(changePositive, changeNegative);
       }

       rotationCurrent.y = horizontalMouse;
       transform.localRotation = Quaternion.Euler(rotationCurrent);
   }
   */



/*
 *     void ReturnCameraToDefault()
    {
        // calculating the target location to move to (location of player)  +  distance
        Vector3 target = player.transform.position;
        target.y += defaultYheight;
        target.z += defaultZdistance;
        target += currentZoom;

        Vector3 targetPosition = target - transform.position;

        float distance = Vector3.Distance(transform.position, target);

        // apply the force
        rb.AddForce(targetPosition.normalized * 650 * Time.deltaTime);                 // add force to the camera towards the target location

        rb.drag = 10 / distance * 1.25f;                              // increase drag (resistance) to camera to slow it down
    }
*/









//     Debug.Log(horizontalMouse);
//     rotationCurrent.y = verticalMouse;
// Debug.Log(Time.time + " " + verticalMouse);
//   transform.localRotation = Quaternion.Euler(rotationCurrent);

/*
/// mouse input
float horizontalMouse = Input.GetAxis("Mouse X") * 3;
horizontalRotationTarget.y += horizontalMouse;

if(playerJoint.grabbing)
{
    if (horizontalRotationTarget.y > clampMax)
    {
        horizontalRotationTarget.y = clampMax;
    }
    else if (horizontalRotationTarget.y < clampMin)
    {
        horizontalRotationTarget.y = clampMin;
    }
}

Vector3 calculatedRotation = Vector3.Lerp(horizontalRotationCurrent, horizontalRotationTarget, 4.25f * Time.deltaTime);

float calculatedDifference = calculatedRotation.y - horizontalRotationCurrent.y;

if (calculatedDifference > 210 * Time.deltaTime)
{
    horizontalRotationCurrent.y += 210 * Time.deltaTime;
}
else if (calculatedDifference < -210 * Time.deltaTime)
{
    horizontalRotationCurrent.y += -210 * Time.deltaTime;
}
else
{
    horizontalRotationCurrent = calculatedRotation;
}

transform.localRotation = Quaternion.Euler(horizontalRotationCurrent);
}
*/

//  List<int> yolo = new List<int> { 10, 10, 10 };


















//    speedPerSec = Vector3.Distance(oldPosition, transform.position) / Time.dealtTime;
//    speed = Vector3.Distance(oldPosition, transform.position);
//   oldPosition = transform.position;

//  Debug.Log(Time.time);

//       horizontalValue = 0;
//      runningValue = 0;
//       forwardValue = 0;

/*
bool wallRunEnabled = false;
if(wallCheckScript.wallObjectsDetected > 0)
{
    wallRunEnabled = true;
}

if(wallRunEnabled && movingForward)
{
    WallRun();
}
else if(movingForward)
{
    ExecuteMovementMidAir();
}
else
{
    PlayerFall();
}    
}
}
*/



//       RaycastHit hit;
//     // Does the ray intersect any objects excluding the player layer
//     if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
//      {
//        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
//        Debug.Log("Did Hit");
//   }

//     if(checkGrab.grabDetected)
//   {
//       Debug.Log("grab detect" + Time.time);
//    }


/*
wallRunTime += Time.deltaTime;
Vector3 initialVelocity = rb.velocity;
Vector3 targetVelocity = Vector3.zero;

// Vector3 sidewaysVector = Vector3.zero;
if(movingSideways)
{
    //  sidewaysVector = transform.right * horizontalInput;
    targetVelocity += transform.right * horizontalInput * 5;
}

Vector3 upwardVector = Vector3.zero;
if(movingForward)
{
    targetVelocity += transform.up * 5;
    rb.velocity = targetVelocity;
}
else
{
    rb.velocity = targetVelocity;
    rb.AddForce(0, -10, 0);
}

*/


// Vector3 move = sidewaysVector + upwardVector;

//   Vector3 targetVelocity = wallMechanic.transform.right

//     if(wallRunTime < wallUpwardsMaxTime)
//     {

//    }
//   else
//   {
//         PlayerFall();
//   }

//   Vector3 currentVelocity = rb.velocity;  
// if(rb.velocity.y < 2.5f)
//   {
//      rb.AddForce(0, 20, 0);
//     Vector3 direction = 
//   }

//     Vector3 target = wallMechanic.transformRight * 20 + transform.up * 20;
//    rb.AddForce(target);




/*
if(Time.time > afterJumpCooldownTimeEnd)
{
    if (currentState == 0)
    {
        StandingState();
    }
    else if (currentState == 1)
    {
        WalkState();
    }
    else if (currentState == -1)
    {
        CrouchState();
    }
    else if (currentState == 2)
    {
        RunState();
    }
    else if (currentState == -2)
    {
        CrouchWalkState();
    }
}
*/


/*
private void FixedUpdate()
{
    if(spacePressed)
    {
        spacePressed = false;

        if(groundCheckScript.groundObjectsDetected > 0 && Time.time > afterJumpCooldownTimeEnd)
        {
            Debug.Log("jump");
            Vector3 move = Vector3.zero;
            if (currentState == 0)           // standing  >  jump up
            {
                move = transform.up * 1000;
            }
            else if(currentState == 1)      // walking  >  jump forward
            {
                float force = currentVelocity * 100;
                move = transform.forward + transform.up * 0.5f;
                move = move.normalized * force;
            }
            else if(currentState == 2)      // walking  >  jump forward
            {
                float force = currentVelocity * 100;
                move = transform.forward + transform.up * 0.5f;
                move = move.normalized * force;
            }

            rb.AddForce(move);
            afterJumpCooldownTimeEnd = Time.time + 1;
        }
    }

    if(groundCheckScript.groundObjectsDetected > 0)
    {
        Vector3 move = transform.forward * forwardMove + transform.right * horizonMove * 0.5f;
        move = move.normalized;
        move = move * currentVelocity;
        rb.velocity = move;
    }
    else
    {
        rb.AddForce(new Vector3(0, -20, 0));
    }
}

*/

/*
void Inputs()
{


    bool moving = false;
    if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
    {
        moving = true;
    }

    if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
    {
        targetState = 2;
    }
    else if(Input.GetKey(KeyCode.LeftControl) && !moving)
    {
        targetState = -1;
    }
    else if(Input.GetKey(KeyCode.LeftControl) && moving)
    {
        targetState = -2;
    }
    else if(moving)
    {
        targetState = 1;
    }
    else
    {
        targetState = 0;
    }

    if(Input.GetKey(KeyCode.W))
    {
        forwardMove = 1;
    }
    else if(Input.GetKey(KeyCode.S))
    {
        forwardMove = -1;
    }
    else
    {
        forwardMove = 0;
    }

    if(Input.GetKey(KeyCode.A))
    {
        horizonMove = -1;
    }
    else if(Input.GetKey(KeyCode.D))
    {
        horizonMove = 1;
    }
    else
    {
        horizonMove = 0;
    }  
}




*/







/*
float percentComplete = transitionProgress / 0.75f;
float curveEvaluate = moveCurve.Evaluate(percentComplete);

currentVelocity = Mathf.Lerp(0, 5, curveEvaluate);

if (curveEvaluate == 1)
{
    currentState = 1;
}
}
else if(targetState < currentState)
{
transitionProgress -= Time.deltaTime;

float
}
}
}
*/


//    Inputs();


//      Vector3 moveDirection = transform.forward * currentVelocity;
//       rb.velocity = moveDirection;




//     if (currentState == 0)
//     {
///         StandState();
//     }
//     Inputs();

/*
if(Input.GetKey(KeyCode.B))
{
    CrouchMechanic(true);
}
else
{
    CrouchMechanic(false);
}
*/


/*
void Inputs()
{
    bool moving = false;                                           // determine if player wants to move
    if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
    {
        moving = true;
    }

    if (Input.GetKey(KeyCode.LeftControl))
    {
        if (moving)
        {
            targetState = -2;
        }
        else
        {
            targetState = -1;
        }
    }
    else if(Input.GetKey(KeyCode.LeftShift) && moving)
    {
        targetState = 2;
    }
    else if(moving)
    {
        targetState = 1;
    }
    else
    {
        targetState = 0;
    }
}
*/





/*
void StandState()
{   

    if(targetState > 0)                               // transition to walk
    {
        if (timeWalkInitiated == -1)
        {
            timeWalkInitiated = Time.time;
        }
        else
        {
            float elapsedTime = Time.time - timeWalkInitiated;
            float percentComplete = elapsedTime / 0.75f;
            float curveEvaluate = moveCurve.Evaluate(percentComplete);

            currentVelocity = Mathf.Lerp(0, 5, curveEvaluate);

            if(percentComplete == 1)
            {
                currentState = 1;
            }
        }
    }
    else if(targetState < 0)                        // transition to crouch
    {
        if(timeCrouchInitiated == -1)
        {
            timeCrouchInitiated = Time.time;
        }
        else
        {
            float elapsedTime = Time.time - timeCrouchInitiated;
            float percentComplete = elapsedTime / 0.75f;
            float curveEvaluate = moveCurve.Evaluate(percentComplete);

            float currentUpperBodyPosition = Mathf.Lerp(1, 0, curveEvaluate);
            playerUpperBody.localPosition = new Vector3(0, currentUpperBodyPosition, 0);

            if(percentComplete == 1)
            {
                currentState = -1;
            }
        }
    }
}
}



*/












/*
 *             elapsedTime = Time.time - runningTime;
            percentageComplete = elapsedTime / 1.25f;
            curveEvaluate = moveCurve.Evaluate(percentageComplete);
            currentForwardVelocity = Mathf.Lerp(velocityWhenRunStart, runVelocity, curveEvaluate);
*/

/*
void Inputs()
{


    if(Input.GetKeyDown(KeyCode.W))
    {
        travelForward = true;
        travelForwardTime = Time.time;
    }
    else if(Input.GetKeyUp(KeyCode.W))
    {
        travelForward = false;
        travelForwardTime = Time.time;
        velocityWhenTravelCancelled = currentForwardVelocity;
    }

    if(Input.GetKeyDown(KeyCode.LeftShift))
    {
        running = true;
    }
    else if(Input.GetKeyUp(KeyCode.LeftShift))
    {
        running = false;
        runningTime = -1;
    }

    if(running && travelForward && runningTime == -1)
    {
        runningTime = Time.time;
        velocityWhenRunStart = currentForwardVelocity;
    }
}

if (Input.GetKey(KeyCode.A))
{
    horizontalInput = -1;
}
else if (Input.GetKey(KeyCode.D))
{
    horizontalInput = 1;
}
else
{
    horizontalInput = 0;
}

if(Input.GetKey(KeyCode.W))
{
    forwardInput = 1;
}
else if(Input.GetKey(KeyCode.S))
{
    forwardInput = -1;
}
else
{
    forwardInput = 0;
}



void Movement()
{
    float percentageComplete;
    float elapsedTime;
    float curveEvaluate;

    if(running)
    {
        elapsedTime = Time.time - runningTime;
        percentageComplete = elapsedTime / 1.25f;
        curveEvaluate = moveCurve.Evaluate(percentageComplete);
        currentForwardVelocity = Mathf.Lerp(velocityWhenRunStart, runVelocity, curveEvaluate);
    }        
    else if(travelForward)
    {
        elapsedTime = Time.time - travelForwardTime;
        percentageComplete = elapsedTime / 1;
        curveEvaluate = moveCurve.Evaluate(percentageComplete);
        currentForwardVelocity = Mathf.Lerp(0, walkVelocity, curveEvaluate);
    }
    else
    {
        elapsedTime = Time.time - travelForwardTime;
        percentageComplete = elapsedTime / 0.55f;
        curveEvaluate = moveCurve.Evaluate(percentageComplete);
        currentForwardVelocity = Mathf.Lerp(velocityWhenTravelCancelled, 0, curveEvaluate);
    }
    Vector3 moveDirection = transform.right * currentSideVelocity + transform.forward * currentForwardVelocity;
    rb.velocity = moveDirection;
}

void CrouchMechanic(bool moveDown)
{
    if(moveDown)
    {
        playerUpperBody.position = Vector3.zero;
    }
    else
    {
        playerUpperBody.localPosition = new Vector3(0, 1, 0);
    }
}

}

Rigidbody rb;
int forward = 0;
int horizontal = 0;
bool running = false;

float turnX = 0;
float turnY = 0;

float mouseX;
float mouseY;

float gravityForce = 100;

public bool onGround;

// Start is called before the first frame update
void Start()
{
    rb = GameObject.Find("Player").GetComponent<Rigidbody>();
}

// Update is called once per frame
void Update()
{
    DetermineInputs();

    transform.localRotation = Quaternion.Euler(-turnY, 0, 0);
    rb.transform.localRotation = Quaternion.Euler(0, turnX, 0);
}

private void FixedUpdate()
{
    Vector3 move = rb.transform.right * horizontal + transform.forward * forward;
    rb.velocity = move;

    if(!onGround)
    {
        rb.AddForce(0, -gravityForce, 0);
    }
}

void DetermineInputs()
{
    mouseX = Input.GetAxis("Mouse X");
    mouseY = Input.GetAxis("Mouse Y");

    turnX += mouseX;
    turnY += mouseY;

    if (turnY > 45)
    {
        turnY = 45;
    }
    else if (turnY < -45)
    {
        turnY = -45;
    }

    if (Input.GetKey(KeyCode.A))
    {
        horizontal = -1;
    }
    else if (Input.GetKey(KeyCode.D))
    {
        horizontal = 1;
    }
    else
    {
        horizontal = 0;
    }

    if (Input.GetKey(KeyCode.W))
    {
        forward = 1;
    }
    else if (Input.GetKey(KeyCode.S))
    {
        forward = -1;
    }
    else
    {
        forward = 0;
    }

    if (Input.GetKey(KeyCode.LeftShift))
    {
        running = true;
    }
    else
    {
        running = false;
    }
}
}
*/





/*

float horizontalCalculate;
float forwardCalculate;

if(horizontalValue < 0)
{
    horizontalCalculate = walkCurve.Evaluate(-horizontalValue);
    horizontalCalculate = -horizontalCalculate;
}
else if(horizontalValue > 0)
{
    horizontalCalculate = walkCurve.Evaluate(horizontalValue);
}
else
{
    horizontalCalculate = 0;
}

if(forwardValue < 0)
{
    forwardCalculate = walkCurve.Evaluate(-forwardValue);
    forwardCalculate = -forwardCalculate;
}
else if(forwardValue > 0)
{
    forwardCalculate = walkCurve.Evaluate(forwardValue);
}
else
{
    forwardCalculate = 0;
}


float runCalculate = runCurve.Evaluate(runningValue);                           // calculating running vector
runVector = transform.forward * runCalculate * runningVelocity;



float tempHorizontal = horizontalCalculate;
float tempForward = forwardCalculate;


if(tempHorizontal < 0)
{
    tempHorizontal = -tempHorizontal;
}

if(tempForward < 0)
{
    tempForward = -tempForward;
}

if(tempHorizontal + tempForward > 1)
{
    Vector2 toNormalise;
    toNormalise.x = horizontalCalculate;
    toNormalise.y = forwardCalculate;
    toNormalise = toNormalise.normalized;

    forwardVector = transform.forward * toNormalise.y * walkingVelocity;
    rightVector = transform.right * toNormalise.x * walkingVelocity;
  //  move = transform.right * toNormalise.x + transform.forward * toNormalise.y;
//     move = move * walkingVelocity;
}
else
{
//         forwardVector = 
//       move = transform.right * horizontalCalculate + transform.forward * forwardCalculate;
//       move = move * walkingVelocity;
}

*/
//     move += runVector;


//   move += runVector;
//    Debug.Log(runVector);

//    rb.velocity = move;


/*     float tempRun = runningValue;
     Debug.Log(tempRun);
     float runCalculate;
     if(tempRun < 0)
     {
         Debug.Log("if");
         tempRun = -tempRun;
         runCalculate = runCurve.Evaluate(tempRun);
         move += transform.forward * runCalculate * runningVelocity;
     }
     else if(tempRun > 1)
     {
         Debug.Log("else");
         runCalculate = runCurve.Evaluate(tempRun);
         move += transform.forward * runCalculate * runningVelocity;
     }
*/




// Vector3 move = transform.right * horizontalValue * walkingVelocity;
//  rb.velocity = move;
/*
Vector3 move = transform.forward * forwardValue + transform.right * horizontalValue;
move = move.normalized;

float curveEvaluate = sCurve.Evaluate(currentVelocityValue);
float velocity = Mathf.Lerp(0, maxVelocity, curveEvaluate);

move = move.normalized;
rb.velocity = move * velocity;
*/

/*

if (movingForward && running)                   // player velocity
{
    currentVelocityValue += 0.025f;
    if (currentVelocityValue > 1)
    {
        currentVelocityValue = 1;
    }
}
else if (horizontalInput != 0 || forwardInput != 0)
{
    if (currentVelocityValue < 0.5f)
    {
        currentVelocityValue += 0.025f;
        if (currentVelocityValue > 0.5f)
        {
            currentVelocityValue = 0.5f;
        }
    }
    else if (currentVelocityValue > 0.5f)
    {
        currentVelocityValue -= 0.025f;
        if (currentVelocityValue < 0.5f)
        {
            currentVelocityValue = 0.5f;
        }
    }
}
else
{
    if (currentVelocityValue > 0.5)
    {
        currentVelocityValue -= 0.025f;
    }
    else if(currentVelocityValue > 0)
    {
        currentVelocityValue -= 0.025f;
        if(currentVelocityValue < 0)
        {
            currentVelocityValue = 0;
        }
    }
}



if (horizontalInput == 1)                                   // direction values
{
    if (horizontalValue < 0.25f)
    {
        horizontalValue += 0.025f;
        if (horizontalValue > 0.25f)
        {
            horizontalValue = 0.25f;
        }
    }
}
else if (horizontalInput == -1)
{
    if (horizontalValue > -0.25f)
    {
        horizontalValue -= 0.025f;
        if (horizontalValue < -0.25f)
        {
            horizontalValue = -0.25f;
        }
    }
}
else                                                // reset the values when no input
{
    if (horizontalValue > 0)
    {
        horizontalValue -= 0.025f;
        if (horizontalValue < 0)
        {
            horizontalValue = 0;
        }
    }
    else if (horizontalValue < 0)
    {
        horizontalValue += 0.025f;
        if (horizontalValue > 0)
        {
            horizontalValue = 0;
        }
    }
}

                                                     // back and forth values
if (forwardInput == 1)
{
    if (forwardValue < 0.5f)
    {
        forwardValue += 0.025f;
        if (forwardValue > 0.5f)
        {
            forwardValue = 0.5f;
        }
    }
}
else if (forwardInput == -1)
{
    if (forwardValue > -0.5f)
    {
        forwardValue -= 0.025f;
    }
}
else
{
    if (forwardValue > 0)
    {
        forwardValue -= 0.025f;
        if (forwardValue < 0)
        {
            forwardValue = 0;
        }
    }
    else if (forwardValue < 0)
    {
        forwardValue += 0.025f;
        if (forwardValue > 0)
        {
            forwardValue = 0;
        }
    }
}
}
*/