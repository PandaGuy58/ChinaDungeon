using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

public class Torch : MonoBehaviour
{
    //Color
    [SerializeField]
    public Color startColour;
    public float colourChangeOffset;
    public float colourTransitionScrollSpeed;
    public float colourTransitionJumpScale;

    //Range
    [SerializeField]
    public float rangeBase = 0.5f;
    public float rangeJumpScale = 0.1f;
    public float rangeScrollSpeed = 1;

    //Intensity
    [SerializeField]
    public float intensityBase = 0.5f;
    public float intensityJumpScale = 0.1f;
    public float intensityScrollSpeed = 1;

    //Objects
    [SerializeField]
    public Light compLight;
    public GameObject reallightObject;
    public GameObject bakedlightObject;

    //Position
    [SerializeField]
    public float positionScrollSpeed = 1;
    public float positionJumpScale = 0.25f;
    Vector3 initialPositionValue;

    int randomScrollVal;
    public bool inZone;

    void Update()
    {
        CheckIfInZone();
        if (inZone)
        {
            reallightObject.SetActive(true);
            bakedlightObject.SetActive(false);
            CalculateIntensity();
            CalculatePosition();
            CalculateColour();
            CalculateRange();
        }
        if (!inZone)
        {
            reallightObject.SetActive(false);
            bakedlightObject.SetActive(true);
        }
    }

    private void Start()
    {
        randomScrollVal = Random.Range(0, 100);
        initialPositionValue = reallightObject.transform.localPosition;
        reallightObject.SetActive(false);
        bakedlightObject.SetActive(true);
    }

    void CalculateIntensity()
    {
        float calculate = (intensityJumpScale * Mathf.PerlinNoise(randomScrollVal + Time.time * intensityScrollSpeed, 1f + Time.time * intensityScrollSpeed));
        if (calculate < 0)
        {
            calculate = -calculate;
        }
        calculate = intensityBase + calculate;
        compLight.intensity = calculate;
    }

    void CalculatePosition()
    {
        float x = Mathf.PerlinNoise(randomScrollVal + Time.time * positionScrollSpeed, 1 + Time.time * positionScrollSpeed) - 0.5f;
        float y = Mathf.PerlinNoise(randomScrollVal + Time.time * positionScrollSpeed, 1 + Time.time * positionScrollSpeed) - 0.5f;
        float z = Mathf.PerlinNoise(4 + Time.time * positionScrollSpeed, 5 + Time.time * positionScrollSpeed) - 0.5f;

        Vector3 calculatePostion = initialPositionValue;
        calculatePostion.x += x * positionJumpScale;
        calculatePostion.y += y * positionJumpScale;
        calculatePostion.z += z * positionJumpScale;


        reallightObject.transform.localPosition = calculatePostion;
    }
    void CalculateColour()
    {
        float r = Mathf.PerlinNoise(colourChangeOffset + Time.time * colourTransitionScrollSpeed, colourChangeOffset + 1 + Time.time * colourTransitionScrollSpeed) - 0.5f;
        float g = Mathf.PerlinNoise(colourChangeOffset + 2 + Time.time * colourTransitionScrollSpeed, colourChangeOffset + 3 + Time.time * colourTransitionScrollSpeed) - 0.5f;
        float b = Mathf.PerlinNoise(colourChangeOffset + 4 + Time.time * colourTransitionScrollSpeed, colourChangeOffset + 5 + Time.time * colourTransitionScrollSpeed) - 0.5f;

        Color targetColour = startColour;
        targetColour.r += r * colourTransitionJumpScale;
        targetColour.g += g * colourTransitionJumpScale;
        targetColour.b += b * colourTransitionJumpScale;
        compLight.color = targetColour;
    }

    void CalculateRange()
    {
        float calculate = (rangeJumpScale * Mathf.PerlinNoise(randomScrollVal + Time.time * rangeScrollSpeed, 1f + Time.time * rangeScrollSpeed));
        if (calculate < 0)
        {
            calculate = -calculate;
        }
        calculate = intensityBase + calculate;
        compLight.range = calculate;
    }

    void CheckIfInZone()
    {

    }
}
