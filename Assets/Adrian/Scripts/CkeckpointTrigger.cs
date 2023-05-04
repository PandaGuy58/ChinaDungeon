using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CkeckpointTrigger : MonoBehaviour
{
    public bool once;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Checkpoints.checkpointID += 1;
            GameObject.Find("GameMaster").GetComponent<Checkpoints>().checkpointPositions.Add(this.gameObject.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (once)
        {
            GameObject.Destroy(this);
        }
    }
}
