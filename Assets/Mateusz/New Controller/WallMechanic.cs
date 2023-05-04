using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMechanic : MonoBehaviour
{
    public Vector3 jumpDirection;                   // store the direction for player to jump off the wall

    public bool xplusone;
    public bool xminusone;
    public bool zplusone;
    public bool zminusone;

    public bool travelX;
    public bool travelZ;


        List<Vector3> directions= new List<Vector3>() { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 0, -1) };





    public bool alternative = false;
    void Start()
    {
        if (xplusone) 
        {
            jumpDirection = directions[0];
            //jumpDirection = transform.forward;
        }
        else if(xminusone)
        {
            jumpDirection = directions[1];
            //jumpDirection = transform.forward * -1;
        }    
        else if(zplusone)
        {
            jumpDirection = directions[2];
          //  jumpDirection = GameObject.Find("GameMaster").GetComponent<GameMaster>().directions[2];
            //jumpDirection = transform.right * -1;
        }
        else if(zminusone)
        {
            jumpDirection = directions[3];
         //   jumpDirection = GameObject.Find("GameMaster").GetComponent<GameMaster>().directions[3];
            //jumpDirection = transform.right;
        }     
    }
}
