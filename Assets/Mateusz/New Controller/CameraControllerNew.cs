using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerNew : MonoBehaviour
{
    public HandsControllerNew handsControl;     // camera
    public Transform bodyTargetPosition;
    public Transform handsTargetPosition;

    public Transform hands;
    public Transform handsTarget;

    public Transform playerBody;

    Vector3 currentRotation = Vector3.zero;
    public Vector3 startRotation;

    public AnimationCurve curve;

    public CharacterControllerNew characterController;

    public float horizontalTheoreticalMouse;

 //   public bool zPositive;
 //   public bool zNegative;

  //  public bool xPositive;
  //  public bool xNegative;


    // Update is called once per frame
    void Update()
    {

        CalculateTheoreticalCameraRotation();

        transform.eulerAngles = currentRotation;

        if (handsControl.handsInControl)
        {


            Vector3 currentPosition = transform.position;                                                       // camera lerp
            currentPosition = Vector3.Lerp(currentPosition, handsTargetPosition.position, Time.deltaTime * 10);
            transform.position = currentPosition;


            if (Time.time > handsControl.timeGrabInitiated + handsControl.currentTargetTime)
            {
                currentRotation.x -= Input.GetAxis("Mouse Y");
                currentRotation.y += Input.GetAxis("Mouse X");

                transform.eulerAngles = currentRotation;

            }
            else
            {
                currentRotation = new Vector3(
                              Mathf.LerpAngle(startRotation.x, handsControl.currentLerpRotationTarget.x, handsControl.curveEvaluate),
                                Mathf.LerpAngle(startRotation.y, handsControl.currentLerpRotationTarget.y, handsControl.curveEvaluate),
                                  Mathf.LerpAngle(startRotation.z, handsControl.currentLerpRotationTarget.z, handsControl.curveEvaluate));
                transform.eulerAngles = currentRotation;
            }

        }
        else if (characterController.pushBody)
        {
            Vector3 currentPosition = transform.position;                                                       // camera lerp
            currentPosition = Vector3.Lerp(currentPosition, bodyTargetPosition.position, Time.deltaTime * 10);
            transform.position = currentPosition;


            float timePassed = Time.time - handsControl.pushUpTime;
            float curveEvaluate = curve.Evaluate(timePassed);

            currentRotation = new Vector3(
              Mathf.LerpAngle(startRotation.x, handsControl.currentLerpRotationTarget.x, curveEvaluate),
                Mathf.LerpAngle(startRotation.y, handsControl.currentLerpRotationTarget.y, curveEvaluate),
                  Mathf.LerpAngle(startRotation.z, handsControl.currentLerpRotationTarget.z, curveEvaluate));
            transform.eulerAngles = currentRotation;
        }
        else
        {

            Vector3 currentPosition = transform.position;                                                       // camera lerp
            currentPosition = Vector3.Lerp(currentPosition, bodyTargetPosition.position, Time.deltaTime * 10);
            transform.position = currentPosition;

            hands.transform.position = handsTarget.position;

            currentRotation.x -= Input.GetAxis("Mouse Y");
            currentRotation.y += Input.GetAxis("Mouse X");

            transform.eulerAngles = currentRotation;

            Vector3 playerBodyRotation = Vector3.zero;
            playerBodyRotation.y = currentRotation.y;
            playerBody.eulerAngles = playerBodyRotation;



        }
    }

    void CalculateTheoreticalCameraRotation()
    {
        if (currentRotation.y > 360)
        {
            horizontalTheoreticalMouse = currentRotation.y - 360;
            while (horizontalTheoreticalMouse > 360)
            {
                horizontalTheoreticalMouse = horizontalTheoreticalMouse - 360;
            }
        }
        else if (currentRotation.y < 0)
        {
            horizontalTheoreticalMouse = currentRotation.y + 360;
            while (horizontalTheoreticalMouse < 0)
            {
                horizontalTheoreticalMouse = horizontalTheoreticalMouse + 360;
            }
        }
        else
        {
            horizontalTheoreticalMouse = currentRotation.y;
        }



    }

    public void ApplyCameraTilt(float zVal)
    {
       // Debug.Log(Time.time + " z" + zVal);
        Vector3 cameraAngle = gameObject.transform.eulerAngles;
        cameraAngle.z = zVal;
        gameObject.transform.eulerAngles = cameraAngle;

     //   Debug.Log(gameObject.transform.eulerAngles + " " + Time.time);
    }


}






















    /*

    void DetermineTilt()
    {
        Vector3 currentPlayerPos = playerTransform.position;
        if (wallX)
        {
            float change = currentPlayerPos.x - recentPlayerPos.x;
            if (change > 0)
            {
                tiltRight = false;
                tiltLeft = true;

                if (characterController.alternativeCamera)
                {
                    if (characterController.alternativeCamera)
                    {
                        tiltRight = true;
                        tiltLeft = false;
                    }
                }
            }
            else if (change < 0)
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
        else if (wallZ)
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

}

    */



/*
void CameraRotate()
{



}
*/




/*
 *             Vector3 handsPosition = handsArea.transform.position;
            handsArea.transform.position = Vector3.Lerp(handsPosition, playerGrab.transform.position, Time.deltaTime * 50f);

            Vector3 targetAngle = handsArea.transform.eulerAngles;
            Vector3 currentAngle = transform.eulerAngles;

            currentAngle = new Vector3(
               Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
               Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
               Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));


            handsArea.transform.eulerAngles = currentAngle;

*/