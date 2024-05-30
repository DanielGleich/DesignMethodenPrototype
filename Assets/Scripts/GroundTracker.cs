using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTracker : MonoBehaviour
{
    PlayerMovement player;

    private void Awake()
    {
        player = transform.parent.gameObject.GetComponent<PlayerMovement>();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("tStay");
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            player.SetGrounded(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("tExit");
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            player.SetGrounded(false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            player.SetGrounded(true);
        }
        Debug.Log("cStay");
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            player.SetGrounded(false);
        }
        Debug.Log("cExit");
        
    }
}
