using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTracker : MonoBehaviour
{
    PlayerMovement player;

    //private void Awake()
    //{
    //    player = transform.parent.gameObject.GetComponent<PlayerMovement>();
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
    //        player.SetGrounded(true);
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        player.SetGrounded(true);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        player.SetGrounded(false);
    //    }
    //}
}
