using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsControllerNew : MonoBehaviour
{
    public bool handsInControl = false;
    GameObject edgeObject;
    Edge edge;
    List<Transform> grabPoints;
    int currentEdgeIndex;

    Vector3 currentLerpPositionTarget;
    public Vector3 currentLerpRotationTarget;

    public float timeGrabInitiated = -1;
    public Transform playerBody;

    public CharacterControllerNew characterController;


    Camera cam;
    GameObject currentRaycast;
    float timeFirstRaycast;
    public LaunchArea launchArea;
    public AnimationCurve curve;
    public CameraControllerNew cameraController;
    public float curveEvaluate;

    float aDown;
    float dDown;
    float wDown;

    float jumpGrabTime;
    float launchGrabTime;
    float moveGrabPointTime;

    Vector3 startPosition;
    Vector3 startRotation;

    public float currentTargetTime;

    public float pushUpTime;



    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (handsInControl)
        {
            HandsInControl();
            RaycastEdge();
            MoveToGrabPointInput();
            PushUpInput();
        }
        else
        {
            BodyInControl();
        }
    }

    void HandsInControl()
    {
        float timePassed = Time.time - timeGrabInitiated;


        if (Time.time < jumpGrabTime + 1)
        {
            currentTargetTime = 1;
        }
        else if (Time.time < launchGrabTime + 2)
        {
            currentTargetTime = 2;
            timePassed = timePassed / 2;            // twice as long (2s)
        }
        else if (Time.time < moveGrabPointTime + 1)
        {
            currentTargetTime = 1;
            timePassed = timePassed * 2;
        }

        curveEvaluate = curve.Evaluate(timePassed);

        Vector3 currentPosition = Vector3.Lerp(startPosition, currentLerpPositionTarget, curveEvaluate);
        transform.position = currentPosition;

        Vector3 currentRotation = new Vector3(
            Mathf.LerpAngle(startRotation.x, currentLerpRotationTarget.x, curveEvaluate),
            Mathf.LerpAngle(startRotation.y, currentLerpRotationTarget.y, curveEvaluate),
            Mathf.LerpAngle(startRotation.z, currentLerpRotationTarget.z, curveEvaluate));
        transform.eulerAngles = currentRotation;
    }

    void BodyInControl()
    {
        Vector3 currentRotation = transform.eulerAngles;
        Vector3 rotationTarget = playerBody.eulerAngles;
        currentRotation = new Vector3(
            Mathf.LerpAngle(currentRotation.x, currentRotation.x, Time.deltaTime),
            Mathf.LerpAngle(currentRotation.y, rotationTarget.y, Time.deltaTime * 10),
            Mathf.LerpAngle(currentRotation.z, currentRotation.z, Time.deltaTime));
        transform.eulerAngles = currentRotation;
    }

    void RaycastEdge()
    {
        if (Time.time > timeGrabInitiated + 2)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit[] raycastHitGrabObject = Physics.RaycastAll(ray, 100);

            GameObject target = null;

            for (int i = 0; i < raycastHitGrabObject.Length; i++)
            {
                GameObject temp = raycastHitGrabObject[i].transform.gameObject;
                if (temp != edgeObject && launchArea.nextGrabObject.Contains(temp))
                {
                    target = temp;
                }

            }

            if (target != null)
            {
                if (currentRaycast != target)
                {
                    currentRaycast = target;
                    timeFirstRaycast = Time.time;
                }
                else if (Time.time > timeFirstRaycast + 1)
                {
                    edgeObject = target;
                    edge = target.GetComponent<Edge>();
                    grabPoints = edge.grabPoint;
                    timeGrabInitiated = Time.time;


                    Vector3 startPoint = cam.transform.position;
                    int shortestIndex = -1;
                    float shortestDistance = -1;
                    Vector3 targetVector;
                    float distance = -1;


                    for (int i = 0; i < grabPoints.Count; i++)
                    {
                        targetVector = grabPoints[i].position;
                        distance = Vector3.Distance(startPoint, targetVector);

                        if (i == 0)
                        {
                            shortestIndex = i;
                            shortestDistance = distance;
                        }
                        else if (distance < shortestDistance)
                        {
                            shortestIndex = i;
                            shortestDistance = distance;
                        }
                    }


                    currentLerpPositionTarget = grabPoints[shortestIndex].position;
                    currentLerpRotationTarget = grabPoints[shortestIndex].eulerAngles;

                    currentEdgeIndex = shortestIndex;
                    startPosition = transform.position;
                    startRotation = transform.eulerAngles;
                    launchGrabTime = Time.time;

                    cameraController.startRotation = cameraController.transform.eulerAngles;
                    curveEvaluate = 0;
                }
            }
        }
    }

    void PushUpInput()
    {
        if(edge.pushUp && Time.time > timeGrabInitiated + 1)
        {
            if(wDown > 0.5f)
            {
                characterController.pushBody = true;
                characterController.PushUp(true);

                handsInControl = false;
                pushUpTime = Time.time;
                wDown = 0;
            }
            else if(Input.GetKeyDown(KeyCode.W))
            {
                wDown = 0;
            }
            else if(Input.GetKey(KeyCode.W))
            {
                wDown += Time.deltaTime;
            }

        }
    }



    void MoveToGrabPointInput()
    {
        if (Time.time > timeGrabInitiated + 1)
        {
            if (aDown > 0.5f)
            {
                aDown = -1;

                if (currentEdgeIndex != 0)
                {
                    startPosition = transform.position;
                    startRotation = transform.eulerAngles;

                    timeGrabInitiated = Time.time;
                    moveGrabPointTime = Time.time;

                    currentEdgeIndex--;
                    currentLerpPositionTarget = grabPoints[currentEdgeIndex].position;
                    cameraController.startRotation = cameraController.transform.eulerAngles;

                    curveEvaluate = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                aDown = 0;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                aDown += Time.deltaTime;
            }
            else if (dDown > 0.5f)
            {
                dDown = -1;
                if (currentEdgeIndex != grabPoints.Count - 1)
                {
                    startPosition = transform.position;
                    startRotation = transform.eulerAngles;

                    timeGrabInitiated = Time.time;
                    moveGrabPointTime = Time.time;

                    currentEdgeIndex++;
                    currentLerpPositionTarget = grabPoints[currentEdgeIndex].position;
                    cameraController.startRotation = cameraController.transform.eulerAngles;

                    curveEvaluate = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                dDown = 0;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dDown += Time.deltaTime;
            }
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Edge") && !handsInControl)
        {
            handsInControl = true;
            jumpGrabTime = Time.time;

            edgeObject = other.gameObject;
            edge = other.gameObject.GetComponent<Edge>();
            grabPoints = edge.grabPoint;


            timeGrabInitiated = Time.time;


            Vector3 startPoint = cam.transform.position;
            int shortestIndex = -1;
            float shortestDistance = -1;
            Vector3 target;
            float distance = -1;


            for (int i = 0; i < grabPoints.Count; i++)
            {
                target = grabPoints[i].position;
                distance = Vector3.Distance(startPoint, target);

                if (i == 0)
                {
                    shortestIndex = i;
                    shortestDistance = distance;
                }
                else if (distance < shortestDistance)
                {
                    shortestIndex = i;
                    shortestDistance = distance;
                }
            }


            currentLerpPositionTarget = grabPoints[shortestIndex].position;
            currentLerpRotationTarget = grabPoints[shortestIndex].eulerAngles;
            currentEdgeIndex = shortestIndex;
            startPosition = transform.position;
            startRotation = transform.eulerAngles;

            cameraController.startRotation = cameraController.transform.eulerAngles;
            curveEvaluate = 0;


        }
    }
}

  