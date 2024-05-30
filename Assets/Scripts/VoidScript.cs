using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) { 
            GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>().ReloadScene();
        }
    }
}
