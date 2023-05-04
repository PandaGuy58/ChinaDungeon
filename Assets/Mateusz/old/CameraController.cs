using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject playerBody;

    Vector3 rotationCurrent = Vector3.zero;

  //  public bool mouseChangePositive;
 //   public bool mouseChangeNegative;

    public float horizontalMouse = 0;
    public float horizontalTheoreticalMouse = 0;
    float verticalMouse = 0;

    bool tiltLeft;
    bool tiltRight;
    float tiltValue = 0;
    public float maxTilt = 10;
    public AnimationCurve tiltCurve;

    public bool wallX;
    public bool wallZ;

    Transform playerTransform;
    Vector3 recentPlayerPos;

    Rigidbody playerRb;

    bool dropActivated = false;
    float timeDropActivated;
    float targetYDrop;

    public AnimationCurve dropCurve;

    CharacterController characterController;
    float recentYVelocity;

    void Start()
    {
        playerBody = GameObject.Find("Upper Body");
        playerTransform = GameObject.Find("Player").transform;
        recentPlayerPos = playerTransform.position;
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        characterController = GameObject.Find("Player").GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        if(playerRb.velocity.y != 0)
        {
            recentYVelocity = playerRb.velocity.y;
        }
   //     Debug.Log(1 / Time.deltaTime);
        horizontalMouse += Input.GetAxis("Mouse X") * 2;
        horizontalTheoreticalMouse += Input.GetAxis("Mouse X") * 2;
        verticalMouse -= Input.GetAxis("Mouse Y") * 2;

        CameraRotate();
        transform.localRotation = Quaternion.Euler(rotationCurrent);

        DetermineTilt();
        CameraTilt();

        CalculatePosition();
    }

    void DetermineTilt()
    {
        Vector3 currentPlayerPos = playerTransform.position;
        if(wallX)
        {
            float change = currentPlayerPos.x - recentPlayerPos.x;
            if(change > 0)
            {
                tiltRight = false;
                tiltLeft = true;

                if(characterController.alternativeCamera)
                {
                    if (characterController.alternativeCamera)
                    {
                        tiltRight = true;
                        tiltLeft = false;
                    }
                }
            }
            else if(change < 0)
            {
                tiltLeft = false;
                tiltRight = true;

                if (characterController.alternativeCamera)
                {
                    if (characterController.alternativeCamera)
                    {
                        tiltRight = true;
                        tiltLeft = false;
                    }
                }
            }
        }
        else if(wallZ)
        {
            float change = currentPlayerPos.z - recentPlayerPos.z;
            if (change > 0)
            {
                tiltLeft = true;
                tiltRight = false;

                if (characterController.alternativeCamera)
                {
                    tiltLeft = false;
                    tiltRight = true;
                }
            }
            else if (change < 0)
            {
                tiltRight = true;
                tiltLeft = false;

                if (characterController.alternativeCamera)
                {
                    tiltRight = false;
                    tiltLeft = true;
                }
            }
        }
        else
        {
            tiltLeft = false;
            tiltRight = false;
        }

        recentPlayerPos = currentPlayerPos;
    }


    void CameraTilt()
    {
        if (tiltLeft)
        {
            if (tiltValue != -1)
            {
                tiltValue -= 1.5f * Time.deltaTime;
                if (tiltValue < -1)
                {
                    tiltValue = -1;
                }
            }
        }
        else if (tiltRight)
        {
            if (tiltValue != 1)
            {
                tiltValue += 1.5f * Time.deltaTime;
                if (tiltValue > 1)
                {
                    tiltValue = 1;
                }
            }
        }
        else
        {
            if (tiltValue > 0)
            {
                tiltValue -= 1.5f * Time.deltaTime;
                if (tiltValue < 0)
                {
                    tiltValue = 0;
                }
            }
            else if (tiltValue < 0)
            {
                tiltValue += 1.5f * Time.deltaTime;
                if (tiltValue > 0)
                {
                    tiltValue = 0;
                }
            }
        }

        float calculateTilt;
        if (tiltValue > 0)
        {
            calculateTilt = tiltCurve.Evaluate(tiltValue) * maxTilt;
        }
        else if (tiltValue < 0)
        {
            calculateTilt = tiltCurve.Evaluate(tiltValue * -1) * -maxTilt;
        }
        else
        {
            calculateTilt = 0;
        }

        rotationCurrent.z = calculateTilt;
    }

    void CameraRotate()
    {

        if(horizontalMouse > 360)
        {
   //        Debug.Log(Time.time + "if");
            horizontalTheoreticalMouse = horizontalMouse - 360;
            while(horizontalTheoreticalMouse > 360)
            {
                horizontalTheoreticalMouse = horizontalTheoreticalMouse - 360;
            }
        }
        else if(horizontalMouse < 0)
        {
            horizontalTheoreticalMouse = horizontalMouse + 360;
            while(horizontalTheoreticalMouse < 0)
            {
                horizontalTheoreticalMouse = horizontalTheoreticalMouse + 360;
            }
        }
        else
        {
            horizontalTheoreticalMouse = horizontalMouse;
        }


   //     if(horizontalTheoreticalMouse > 360)
   //     {
   //         horizontalTheoreticalMouse = horizontalMouse - 360;
  //      }
   //     else if(horizontalTheoreticalMouse < 0)
    //    {
  //          horizontalTheoreticalMouse = 360 - horizontalMouse;
    //    }

     //   if (horizontalTheoreticalMouse > 60)
     ///   {
    ///        horizontalTheoreticalMouse = 60;
    //    }
     //   else if (horizontalTheoreticalMouse < -90)
      //  {
       //     horizontalTheoreticalMouse = -90;
     //   }

        rotationCurrent.x = verticalMouse;
        rotationCurrent.y = horizontalMouse;

    }

    public void CalculatePosition()
    {

        if(dropActivated)
        {
            float curveEvaluate = dropCurve.Evaluate(Time.time - timeDropActivated);
            Vector3 defaultPosition = playerBody.transform.position;
            defaultPosition.y -= curveEvaluate * targetYDrop;
            transform.position = defaultPosition;

            if(Time.time - timeDropActivated > 0.7f)
            {
               // Debug.Log("activate :" + Time.time);
                dropActivated = false;
            }
        }
        else
        {
            transform.position = playerBody.transform.position;     // location
        }
    }






    public void ExecuteCameraDrop()
    {
    //    Debug.Log(Time.time + " : " + dropActivated);
      //  float yVelocity = playerRb.velocity.y;

        if (!dropActivated)
        {
        //    Debug.Log("dropActivated :" + Time.time);
            dropActivated = true;
            timeDropActivated = Time.time;
            targetYDrop = -recentYVelocity * 0.1f;

        }
    }
}





/*
private void FixedUpdate()
{

float difference;                                       // prevent extreme values
if (rotationCurrent.y > 360)
{
    Debug.Log(rotationCurrent.y);
    difference = 360 - rotationCurrent.y;

    rotationCurrent.y = difference;
}
else if (rotationCurrent.y < 0)
{
    difference = rotationCurrent.y;
    rotationCurrent.y = 360 - difference;
}

transform.localRotation = Quaternion.Euler(rotationCurrent);

transform.position = playerBody.transform.position;


} */


/*
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


if(horizontalMouse < 0)
{
    float remainder = horizontalMouse % -360;
    calculatedY = 360 + remainder;
}
else if(horizontalMouse > 360)
{
    calculatedY = horizontalMouse % 360;
}
else
{
    calculatedY = horizontalMouse;
}


  */













/*
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




public void InitialiseGrab(float maxY, float minY)
{
grabbing = true;
clampMax = maxY;
clampMin = minY;

//      Debug.Log("initialise grab camera");
}

public void FinaliseGrab()
{
grabbing = false;
}



public void GrabbingClamp(bool positiveChange, bool negativeChange)
{
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
else if(positiveChange)
{
    if(horizontalMouse < calculateClamp && horizontalMouse > clampMax)
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






/*

rotationTarget.y += horizontalMouse;
rotationTarget.x -= verticalMouse;

if(rotationTarget.x > 80)                                                               // clamp
{
rotationTarget.x = 80;
}
else if(rotationTarget.x < -80)
{
rotationTarget.x = -80;
}

if(playerGrabbing)
{
if(rotationTarget.y > clampMax)
{
    rotationTarget.y = clampMax;
}
else if(rotationTarget.y < clampMin)
{
    rotationTarget.y = clampMin;
}
}


Vector3 calculatedRotation = Vector3.Lerp(rotationCurrent, rotationTarget, 5 * Time.deltaTime);


float calculatedDifference = calculatedRotation.y - rotationCurrent.y;                                      // calculating current y rotation

if (calculatedDifference > 350 * Time.deltaTime)                                                            // preventing unlimited speed
{
rotationCurrent.y += 350 * Time.deltaTime;
}
else if (calculatedDifference < -350 * Time.deltaTime)
{
rotationCurrent.y += -350 * Time.deltaTime;
}
else
{
rotationCurrent = calculatedRotation;
}


calculatedDifference = calculatedRotation.x - rotationCurrent.x;                                            // calculating current y rotation

if (calculatedDifference > 350 * Time.deltaTime)                                                            // preventing unlimited speed
{
rotationCurrent.x += 350 * Time.deltaTime;
}
else if(calculatedDifference < -350 * Time.deltaTime)
{
rotationCurrent.x += -350 * Time.deltaTime;
}
else
{
rotationCurrent = calculatedRotation;
}

float difference;                                       // prevent extreme values
if (rotationCurrent.y > 360)
{
Debug.Log(rotationCurrent.y);
difference = 360 - rotationCurrent.y;

rotationCurrent.y = difference;
}
else if (rotationCurrent.y < 0)
{
   difference = rotationCurrent.y;
   rotationCurrent.y = 360 - difference;
}
// calculating current x rotation

transform.localRotation = Quaternion.Euler(rotationCurrent);


Debug.Log(rotationCurrent.y);
}
}
*/