using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerNew : MonoBehaviour
{
    bool movingForward = false;                      // travelling
    bool running = false;

    public float horizontalValue = 0;
    public float forwardValue = 0;
    public float runningValue = 0;

    float velocityChange = 0.1f;
    float runningChange = 0.075f;

    int forwardInput = 0;
    int horizontalInput = 0;

    float timeSpaceDown = -1;
    float timeJumped = -1;

    public float wallRunTimeLeft;


    Rigidbody rb;
    public float gravityValue = 10;

    // jump
    public float forwardJumpValue;
    public float upwardJumpValue;

    public AnimationCurve walkCurve;
    public AnimationCurve runCurve;

    public float walkingVelocity;                   // travelling
    public float runningVelocity;

    Vector3 forwardVector;
    Vector3 rightVector;

    Vector3 runForwardVector;
    Vector3 runRightVector;

    Vector3 move;


    float timeLastGrounded;

    public GameObject handsArea;
    public GameObject playerGrab;

    public HandsControllerNew handsControl;
    public GameObject bodyFollowHands;


    public bool pushBody = false;

    public float pushUpValue = 100;                         // pushing up after lerp to edge
    public float pushForwardValue = 100;

    public Vector3 wallJumpDirection;

    public bool wallRunTravelX = false;
    public bool wallRunTravelZ = false;
    public Transform wallTransform;

    public bool alternativeCamera;                          // wall running
    public float wallRunTotalTimeAllowed = 2;

    public float wallRunJumpForwardValue;
    public float wallRunJumpUpValue;
    public float wallRunForwardValue;
    public float wallRunDownwardValue;

    public CameraControllerNew camController;               // camera tilt

    bool tiltRight;
    bool tiltLeft;
    float tiltValue = 0;
    public float currentZRotation;


    public Transform playerTransform;


    public float maxTilt = 10;
    public AnimationCurve tiltCurve;

    Vector3 recentPlayerPos = Vector3.zero;
    bool wallRunning = false;

    public float pushUpForce;
    public float pushForwardForce;

    public CheckPushUp checkPushUp;

    public float midAirForce = 1.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        DetermineTilt();
        CameraTilt();

        if (!handsControl.handsInControl)                      // when hands not in control  >  player body decision
        {
            if(!pushBody)
            {
                Inputs();
            }
        }
        else
        {
            transform.position = bodyFollowHands.transform.position;                // otherwise  >  place body where hands are
            transform.eulerAngles = bodyFollowHands.transform.eulerAngles;
        }
    }

    private void FixedUpdate()
    {
        CalculateVelocityValues();
        if (!handsControl.handsInControl)
        {
            

            if (pushBody)
            {
                PushUp(false);
            }
            else if (DetectWallRunning())
            {
                WallRun();              // wall running
                rb.AddForce(new Vector3(0, gravityValue, 0));
            } 
            else
            {
                rb.AddForce(new Vector3(0, gravityValue, 0));
            }
        }
        else
        {
            wallRunning = false;
        }

        if (timeLastGrounded > Time.time - 0.2f)
        {
            if (checkPushUp.pushUpCounter != 0)
            {
                Vector3 upward = transform.up * pushUpValue * runningValue;
                rb.AddForce(upward);
                Vector3 forward = transform.forward * pushForwardValue * runningValue;
                rb.AddForce(forward);

            }
            else if (timeSpaceDown > Time.time - 0.2f)
            {
                Jump();
            }
            else
            {
                ExecuteMovementOnGround();
            }
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


    void ExecuteMovementMidAir()
    {
        Vector3 move = transform.forward * forwardValue * midAirForce;
        rb.AddForce(move);
    }

    public void PushUp(bool upwards)
    {
    //    Debug.Log(Time.time + " " + upwards);
        if (upwards)
        {
            Debug.Log(Time.time);
            rb.velocity = new Vector3(0, 0, 0);
            Vector3 upward = transform.up * pushUpValue;
            rb.AddForce(upward);
        }
        else
        {
            Vector3 forward = transform.forward * pushForwardValue;
            rb.AddForce(forward);
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
            if (Time.time > timeJumped + 1)
            {
                timeSpaceDown = Time.time;
            }
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

    bool DetectWallRunning()
    {
        if (wallRunTimeLeft > 0 && movingForward && running && Time.time > timeLastGrounded + 0.1f)
        {
            wallRunning = true;
            return true;
        }
        wallRunning = false;
        return false;
    }

    void WallRun()
    {
       // Debug.Log(Time.time);
        Vector3 targetDirection = Vector3.zero;

        if (timeSpaceDown > Time.time - 0.1f)
        {
            timeJumped = Time.time;
            Vector3 jumpDirection = wallJumpDirection * wallRunJumpForwardValue;
            Vector3 upDirection = transform.up * wallRunJumpUpValue;

            rb.AddForce(jumpDirection + upDirection);
            wallRunTimeLeft = 0;
        }
        else
        {

            if (Time.time > timeLastGrounded)
            {
                wallRunTimeLeft -= 0.02f;
            }



            if (wallRunTravelX)
            {
                if (camController.horizontalTheoreticalMouse < 140 && camController.horizontalTheoreticalMouse > 40)
                {
                    targetDirection = wallTransform.forward * wallRunForwardValue + transform.up * wallRunDownwardValue;
                }
                else if (camController.horizontalTheoreticalMouse < 320 && camController.horizontalTheoreticalMouse > 220)
                {
                    targetDirection = wallTransform.forward * -wallRunForwardValue + transform.up * wallRunDownwardValue;
                }

            }

            else if (wallRunTravelZ)
            {
                if(camController.horizontalTheoreticalMouse > 310 || camController.horizontalTheoreticalMouse < 50)
                {
                    targetDirection = wallTransform.forward * wallRunForwardValue + transform.up * -wallRunDownwardValue;
                }
                else if(camController.horizontalTheoreticalMouse < 230 && camController.horizontalTheoreticalMouse > 130)
                {
                    targetDirection = wallTransform.forward * -wallRunForwardValue + transform.up * -wallRunDownwardValue;
                }
            }


            if (targetDirection != Vector3.zero)
            {
                rb.velocity = targetDirection;
            }
            
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

    void Jump()
    {

        timeJumped = Time.time;
        Vector3 move = transform.forward * forwardValue * forwardJumpValue + transform.up * upwardJumpValue;
        rb.AddForce(move);
    }

    void ExecuteMovementOnGround()
    {
        if (Time.time > timeJumped + 0.5f)
        {

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

    void PlayerFall()
    {
        Vector3 currentVelocity = rb.velocity;
        currentVelocity.x = currentVelocity.x / 1.05f;
        currentVelocity.z = currentVelocity.z / 1.05f;

        rb.velocity = currentVelocity;
        rb.AddForce(new Vector3(0, -1, 0));
    }

    void DetermineTilt()
    {
        Vector3 currentPlayerPos = transform.position;
        if (wallRunTravelX && wallRunning)
        {
            float change = currentPlayerPos.x - recentPlayerPos.x;
            if (change > 0)
            {
                tiltRight = true;
                tiltLeft = false;

                    if (alternativeCamera)
                    {
                        tiltRight = false;
                        tiltLeft = true;
                    }

            }
            else if (change < 0)
            {
                tiltLeft = false;
                tiltRight = true;


                    if (alternativeCamera)
                    {
                        tiltRight = true;
                        tiltLeft = false;
                    }

            }
        }
        else if (wallRunTravelZ && wallRunning)
        {
            float change = currentPlayerPos.z - recentPlayerPos.z;
            if (change > 0)
            {
                tiltLeft = true;
                tiltRight = false;

                if (alternativeCamera)
                {
                    tiltLeft = false;
                    tiltRight = true;
                }
            }
            else if (change < 0)
            {
                tiltRight = true;
                tiltLeft = false;

                if (alternativeCamera)
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

        currentZRotation = calculateTilt;
        camController.ApplyCameraTilt(currentZRotation);
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            timeLastGrounded = Time.time;
            pushBody = false;
        }
    }
}

