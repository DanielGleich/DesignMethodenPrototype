using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{
    [SerializeField] float unjumpableTime;
    [SerializeField] bool customShaderGraph = false;
    [SerializeField] float customShaderAlpha;
    Collider col;
    MeshRenderer render;

    [Header("Materials")]
    [SerializeField] Material jumpableMat;
    [SerializeField] Material unjumpableMat;

    private void Awake()
    {
        col = GetComponent<Collider>();
        render = GetComponent<MeshRenderer>();
        jumpableMat.SetFloat("_Alpha", 1);
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
        if (customShaderGraph)
        {
            jumpableMat.SetFloat("_Alpha", customShaderAlpha);
        }
        else { 
            render.material = unjumpableMat;
        }
        yield return new WaitForSeconds(unjumpableTime);
        col.enabled = true;
        if (customShaderGraph)
        {
            jumpableMat.SetFloat("_Alpha", 1);
        }
        else
        {
            render.material = jumpableMat;
        }
    }
}
