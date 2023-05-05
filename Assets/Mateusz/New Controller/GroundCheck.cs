using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    public float timeLastGrounded;
 //   public float pushTimeAllowed;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            timeLastGrounded = Time.time + 0.1f;
         //   pushTimeAllowed = 0.075f;
        }
    }
}
