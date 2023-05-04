using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFallZoneUp : MonoBehaviour
{
    public Transform fallZone;
    public GameObject player;

    public int heightId;

    public bool phase;

    private void Start()
    {
        heightId = -1;
        phase = false;
    }

    void Update()
    {
        PlayerHeight();
        HeightChanger();
    }

    void PlayerHeight()
    {
        if(player.transform.position.y < -14 && heightId == -1)
        {
            heightId = 0;
            phase = true;
        }

        else if (player.transform.position.y > -14 && player.transform.position.y < 0 && heightId == 0)
        {
            heightId = 1;
        }

        else if (player.transform.position.y > 0 && heightId == 1)
        {
            heightId = 2;
        }

    }

    void HeightChanger()
    {
        switch (heightId)
        {
            case 0:
                Debug.Log("Start");
                fallZone.transform.position = new Vector3(-43.29f, -27.81f, 18.56f);
                break;
            case 1:
                Debug.Log("First");
                fallZone.transform.position = new Vector3(-43.29f, -20.71f, 18.56f);
                break;
            case 2:
                Debug.Log("Second");
                fallZone.transform.position = new Vector3(-43.29f, -3.21f, 18.56f);
                break;
            default:
                break;
        }
    }
}
