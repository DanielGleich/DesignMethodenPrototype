using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{
    [SerializeField] float unjumpableTime;

    [Header("Materials")]
    [SerializeField] Material jumpableMat;
    [SerializeField] Material unjumpableMat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile")) { 
            
        }
    }

    IEnumerator TriggerUnjumpable() { 
        yield return null;
    }
}
