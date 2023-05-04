using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    //  bool grabbing = false;
    public CheckGrab checkGrab;
    public GameObject cam;
    public GameObject hands;
    public GameObject defaultPos;
    public GameObject bodyHands;

    private void Update()
    {
        if (checkGrab.grabObject == null)
        {
            gameObject.transform.parent = cam.transform;
            Vector3 currentPos = gameObject.transform.position;
            Vector3 targetPos = defaultPos.transform.position;
            Vector3 calculatePos = Vector3.Lerp(currentPos, targetPos, 2 * Time.deltaTime);

            transform.position = calculatePos;

            Vector3 currentRotate = transform.localEulerAngles;
            Vector3 calculateRotate = LerpAngleCalc(currentRotate, Vector3.zero);
            transform.localEulerAngles = calculateRotate;
            //      Debug.Log(Time.time);
        }
        else
        {
            gameObject.transform.parent = null;
            Vector3 currentPos = gameObject.transform.position;
            Vector3 targetPos = hands.transform.position;
            Vector3 calculatePos = Vector3.Lerp(currentPos, targetPos, 2 * Time.deltaTime);

            transform.position = calculatePos;

            Vector3 currentRotate = transform.eulerAngles;
            Vector3 targetRotate = hands.transform.eulerAngles;
            targetRotate.z = 0;
            Vector3 calculateRotate = LerpAngleCalc(currentRotate, targetRotate);
            transform.eulerAngles = calculateRotate;



            // gameObject.transform.parent = hands.transform;

        }
    }


    Vector3 LerpAngleCalc(Vector3 currentAngle, Vector3 targetAngle)
    {
        currentAngle = new Vector3(
         Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
         Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
         Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));

        return currentAngle;
    }
}

//}
/*
public bool defaultPosition = true;
public Transform targetPosition;
// Start is called before the first frame update
void Start()
{
   // cameraTransform = GameObject.Find("Main Camera").transform;
}
/*
public bool defaultPosition = true;
public Transform targetPosition;
// Start is called before the first frame update
void Start()
{
// cameraTransform = GameObject.Find("Main Camera").transform;
}

// Update is called once per frame
void Update()
{
    transform.position = targetPosition.position;
//      if(defaultPosition)
//   {
//     Vector3 targetVector = cameraTransform.position;
//     targetVector.y -= 0.5f;
//     transform.position = targetVector;
//  }
}
// Update is called once per frame
void Update()
{
transform.position = targetPosition.position;
//      if(defaultPosition)
//   {
//     Vector3 targetVector = cameraTransform.position;
//     targetVector.y -= 0.5f;
//     transform.position = targetVector;
//  }
}
}

*/


    /*
     *          currentAngle = new Vector3(
             Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
    *.
    */



// Vector3 start = Vector3.zero;
//    Vector3 end = Vector3.zero;
/*
public bool grabbing;

public Transform defaultPosition;
public Transform handsTargetPosition;

public Transform camera;
Rigidbody rb;

public AnimationCurve curve;
public AnimationCurve rotationCurve;
float timePassed = 0;

Vector3 recentRotation;
float timeResetRotation;

public CheckGrab checkGrab;
private void Start()
{
    rb = GetComponent<Rigidbody>();
}
private void FixedUpdate()
{
  //  Debug.Log(Time.time);

    Vector3 defaultPos = defaultPosition.position;
    Vector3 targetPosition = handsTargetPosition.position;
    targetPosition.y -= 1;


    if (grabbing)
    {
        Debug.Log(Time.time);
        transform.parent = null;
        if (timePassed != 1)
        {
            timePassed += Time.deltaTime;
            if (timePassed > 1)
            {
                timePassed = 1;
            }
        }

        float curveEvaluate = curve.Evaluate(timePassed);
        Debug.Log(timePassed);
        Vector3 calculatePosition = Vector3.Lerp(defaultPos, targetPosition, curveEvaluate);
        transform.position = calculatePosition;

    }
    else
    {
        transform.parent = camera;
        if(timePassed != 0)
        {
            timePassed -= Time.deltaTime;
            if (timePassed < 0)
            {
                timePassed = 0;
            }
        }

        float curveEvaluate = curve.Evaluate(timePassed);
        Vector3 calculatePosition = Vector3.Lerp(defaultPos, targetPosition, curveEvaluate);
        transform.position = calculatePosition;

        curveEvaluate = curve.Evaluate(timePassed);
    //    Debug.Log(curveEvaluate);
        Vector3 calculateRotate = Vector3.Lerp(Vector3.zero, recentRotation, curveEvaluate);
        transform.localEulerAngles = calculateRotate;
    }




}

public void CalculateRecentRotation()
{
    recentRotation = transform.localEulerAngles;
    timeResetRotation = Time.time;
}
}


*/
/*
Vector3 current = transform.position;
Vector3 target = handsTargetPosition.position;
target.y -= 1;

Vector3 direction = target - current;
direction = direction.normalized;

float distance = Vector3.Distance(current, target);

rb.AddForce(direction * 15);
rb.drag = 15 / distance * 1.5f;

//   ;

Vector3 current = transform.position;
Vector3 target = handsTargetPosition.position;
Debug.Log(Time.time);
target.y -= +0.7f;
//       target.y -= 2.08f;
current = Vector3.Lerp(current, target, 1 * Time.deltaTime);
transform.position = current;

Vector3 currentAngle = transform.eulerAngles;
Debug.Log(currentAngle);
*/
//  }
//   else
//   {
//     transform.SetParent(camera);


/*
Vector3 current = transform.position;
current = Vector3.Lerp(current, defaultPosition.position, 3 * Time.deltaTime);
transform.position = current;

Vector3 angle = transform.localEulerAngles;
angle = Vector3.Lerp(angle, Vector3.zero, 3 * Time.deltaTime);
transform.localEulerAngles = angle;
*/

//     }

//   }

//}
/*
public bool defaultPosition = true;
public Transform targetPosition;
// Start is called before the first frame update
void Start()
{
   // cameraTransform = GameObject.Find("Main Camera").transform;
}

// Update is called once per frame
void Update()
{
    transform.position = targetPosition.position;
//      if(defaultPosition)
//   {
//     Vector3 targetVector = cameraTransform.position;
//     targetVector.y -= 0.5f;
//     transform.position = targetVector;
//  }
}
}

*/