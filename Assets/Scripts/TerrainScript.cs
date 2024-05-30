using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{
    [SerializeField] float unjumpableTime;
    Collider col;
    MeshRenderer render;

    [Header("Materials")]
    [SerializeField] Material jumpableMat;
    [SerializeField] Material unjumpableMat;

    private void Awake()
    {
        col = GetComponent<Collider>();
        render = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile")) { 
            
        }
    }

    public void SimulateProjectileHit() { 
        StartCoroutine(TriggerUnjumpable());
    }

    IEnumerator TriggerUnjumpable() {
        col.enabled = false;
        render.material = unjumpableMat;
        yield return new WaitForSeconds(unjumpableTime);
        col.enabled = true;
        render.material = jumpableMat;
    }
}
