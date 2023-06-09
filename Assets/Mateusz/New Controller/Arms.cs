using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arms : MonoBehaviour
{
    public Animator animator;                           // essentials
    public HandsControllerNew handsController;
    public AnimationCurve transitionCurve;
    public Transform cameraPosition;
    public Transform bodyPosition;

    float lerpValue = 0;
    public GameObject cam;

    Vector2 animationState = Vector2.zero;


    Vector2 grab = new Vector2(2, 3.5f);                // animation blending lerp vectors
    Vector2 hold = new Vector2(4, 0);
    Vector2 left = new Vector2(2, -3.5f);
    Vector2 push = new Vector2(-2, -3.5f);
    Vector2 release = new Vector2(-4, 0);
    Vector2 right = new Vector2(-2, 3.5f);





    public float grabTime;                  // calculate action times
    public float holdTime;
    public float leftTime;
    public float pushTime;
    public float releaseTime;
    public float rightTime;

    void CalculateAnimate()
    {
        Vector2 targetLerp = Vector2.zero;
        if(grabTime > Time.time)
        {
            targetLerp = grab;
        }
        else if(leftTime > Time.time)
        {
            targetLerp = left;
        }
        else if(pushTime > Time.time)
        {
            targetLerp = push;
        }
        else if(releaseTime > Time.time)
        {
            targetLerp = release;
        } 
        else if(rightTime > Time.time)
        {
            targetLerp = right;
        }
        else if(handsController.handsInControl)
        {
            targetLerp = hold;
        }

        animationState = Vector2.Lerp(animationState, targetLerp, 5 * Time.deltaTime);

        animator.SetFloat("ValX", animationState.x);
        animator.SetFloat("ValY", animationState.y);
    }
    // Update is called once per frame
    void Update()
    {
        CalculateAnimate();

        if (handsController.handsInControl)                                     // hands lerp to edge
        {
            gameObject.transform.parent = bodyPosition;

            Vector3 currentPos = gameObject.transform.position;
            Vector3 targetPos = bodyPosition.transform.position;
            Vector3 calculatePos = Vector3.Lerp(currentPos, targetPos, 2 * Time.deltaTime);

            transform.position = calculatePos;

            Vector3 currentRotate = transform.eulerAngles;
            Vector3 targetRotate = bodyPosition.transform.eulerAngles;
            targetRotate.z = 0;
            Vector3 calculateRotate = LerpAngleCalc(currentRotate, targetRotate);
            transform.eulerAngles = calculateRotate;



            lerpValue += Time.deltaTime;
            if(lerpValue > 1)
            {
                lerpValue = 1;
            }
        }
        else                                                                // hands lerp to body
        {
            gameObject.transform.parent = cam.transform;
            Vector3 currentPos = gameObject.transform.position;
            Vector3 targetPos = cameraPosition.transform.position;
            Vector3 calculatePos = Vector3.Lerp(currentPos, targetPos, 2 * Time.deltaTime);

            transform.position = calculatePos;

            Vector3 currentRotate = transform.localEulerAngles;
            Vector3 calculateRotate = LerpAngleCalc(currentRotate, Vector3.zero);
            transform.localEulerAngles = calculateRotate;


            lerpValue -= Time.deltaTime;
            if(lerpValue < 0)
            {
                lerpValue = 0;
            }
        }

        float curveEvaluate = transitionCurve.Evaluate(lerpValue);


        Vector3 target = Vector3.Lerp(cameraPosition.position, bodyPosition.position, curveEvaluate);
        transform.position = target;
    }

    Vector3 LerpAngleCalc(Vector3 currentAngle, Vector3 targetAngle)            // calculate angle to lerp to
    {
        currentAngle = new Vector3(
         Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
         Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
         Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));

        return currentAngle;
    }
}








